using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Data;
using EfCoreRepository;
using ScottPlot;
using System.Drawing;
using RepairFirm.Shared.Models;

namespace RepairFirm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly IDbRepository _dbRepository;
        public ReportController(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public IActionResult Index()
        {

            if (System.IO.File.Exists("wwwroot/report.pdf"))
            {
                return View();
            }

            #region 3 Chart

            List<DataPoint> dataPoints1 = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();
            List<DataPoint> dataPoints3 = new List<DataPoint>();

            List<List<DataPoint>> points = new List<List<DataPoint>>();
            var temp = _dbRepository.GetDepartmentPayments().GroupBy(x => x.CityName);
            int i = 0, j = 1;
            foreach (var group in temp)
            {
                points.Add(new List<DataPoint>());

                foreach (var item in group)
                {
                    points[i].Add(new DataPoint { Label = j.ToString(), Y = item.TotalPrice });
                    j++;
                }
                ViewData["city" + (i + 1)] = group.Key;
                j = 1;
                i++;
            }

            dataPoints1 = points[0];
            dataPoints2 = points[1];
            dataPoints3 = points[2];

            #endregion
            

            var stream = new FileStream("wwwroot/report.pdf", FileMode.CreateNew);

            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
            document.AddAuthor("Pavlo Somko");
            document.AddTitle("Repair firm analysis report");

            AddGeneralPage(document);
            AddFactTable(document);

            AddOthersTables(document);

            AddDiagram(document, "Popularity of repair types", GenerateImgForRepairsPopular());

            var donats = _dbRepository.GetRepairsByCity().GroupBy(x => x.CityName);
            var repairsByCitiesDatas = new Dictionary<string, List<RepairsByCitiesData>>();
            int newPage = 0;
            foreach (var group in donats)
            {
                repairsByCitiesDatas[group.Key] = new List<RepairsByCitiesData>();
                foreach (var item in group)
                {
                    repairsByCitiesDatas[group.Key].Add(item);
                }
               
            }
            foreach (var item in repairsByCitiesDatas.Keys)
            {
                var dist = repairsByCitiesDatas[item].DistinctBy(x => x.RepairType).ToList();
                foreach (var d in dist)
                {
                    d.RepairCount = repairsByCitiesDatas[item].Where(x => x.RepairType == d.RepairType).Sum(x => x.RepairCount);
                }
                repairsByCitiesDatas[item] = dist;
                AddPie(document, GenerateImgForCity($"Ремонти в                    {item}", repairsByCitiesDatas[item]), newPage % 2);
                newPage++;
            }


            AddDiagram(document, "Amount of contracts in each city", GenerateImgForContractCounts());
            AddDiagram(document, "Countract price in the most popular departments", GenerateImgForContractPrice());
            AddDiagram(document, "Employee count for repair types", GenerateImgForEmployeeCount());

            document.Close();
            writer.Close();


            return View();
        }

        private void AddOthersTables(Document document)
        {
            document.NewPage();
            document.Add(new Paragraph(new Phrase { new Chunk(("Popularity of repair types (in how many contracts it is present)").ToUpper()) })
            {
                Alignment = Element.ALIGN_CENTER,
            });
            var table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
            };

            AddOthersTablesCell(table, "Repair Type", "Repair Count");
            foreach (var item in _dbRepository.GetRepairCountChart())
            {
                AddOthersTablesCell(table, dict1[item.RepairType], item.RepairCount.ToString());
            }
            document.Add(table);

            document.Add(new Paragraph(new Phrase { new Chunk(("The number of signed contracts by city").ToUpper()) })
            {
                Alignment = Element.ALIGN_CENTER,
            });
            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
            };

            AddOthersTablesCell(table, "City Name", "Services Count");
            foreach (var item in _dbRepository.GetDepartmentServices())
            {
                AddOthersTablesCell(table, cities[item.CityName], item.ServicesCount.ToString());
            }
            document.Add(table);


            document.Add(new Paragraph(new Phrase { new Chunk(("Total number of workers for services").ToUpper()) })
            {
                Alignment = Element.ALIGN_CENTER,
            });
            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
            };

            AddOthersTablesCell(table, "Repair Type", "Employee Count");
            foreach (var item in _dbRepository.GetEmployeeForRepairType())
            {
                AddOthersTablesCell(table, dict1[item.RepairType], item.EmpoyeeCount.ToString());
            }
            document.Add(table);


            Dictionary<string, List<RepairPriceForContract>> points = new Dictionary<string, List<RepairPriceForContract>>();
            var temp = _dbRepository.GetDepartmentPayments().GroupBy(x => x.CityName);
            int i = 0, j = 1;
            foreach (var group in temp)
            {
                points[group.Key] = new List<RepairPriceForContract>();

                foreach (var item in group)
                {
                    points[group.Key].Add(new RepairPriceForContract { Index = j.ToString(), TotalPrice = item.TotalPrice });
                    j++;
                }
                j = 1;
            }
            foreach (var gourp in points)
            {
                document.Add(new Paragraph(new Phrase { new Chunk(($"Cost of contracts in the city {cities[gourp.Key]}").ToUpper()) })
                {
                    Alignment = Element.ALIGN_CENTER,
                });
                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10,
                };

                AddOthersTablesCell(table, "Contract Number", "Contract Price");
                foreach (var item in gourp.Value)
                {
                    AddOthersTablesCell(table, item.Index + " contract",  item.TotalPrice.ToString());
                }
                document.Add(table);
            }


            var donats = _dbRepository.GetRepairsByCity().GroupBy(x => x.CityName);
            Dictionary<string, List<RepairsByCitiesData>> repairsByCitiesDatas = new Dictionary<string, List<RepairsByCitiesData>>();
            foreach (var group in donats)
            {
                repairsByCitiesDatas[group.Key] = new List<RepairsByCitiesData>();
                foreach (var item in group)
                {
                    repairsByCitiesDatas[group.Key].Add(item);
                }
            }
            foreach (var item in repairsByCitiesDatas.Keys)
            {
                var dist = repairsByCitiesDatas[item].DistinctBy(x => x.RepairType).ToList();
                foreach (var d in dist)
                {
                    d.RepairCount = repairsByCitiesDatas[item].Where(x => x.RepairType == d.RepairType).Sum(x => x.RepairCount);
                }
                repairsByCitiesDatas[item] = dist;
            }

            foreach (var gourp in repairsByCitiesDatas)
            {
                document.Add(new Paragraph(new Phrase { new Chunk(($"Number and types of services provided in the city {cities[gourp.Key]}").ToUpper()) })
                {
                    Alignment = Element.ALIGN_CENTER,
                });
                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10,
                };

                AddOthersTablesCell(table, "Repair Type", "Repair Count");
                foreach (var item in gourp.Value)
                {
                    AddOthersTablesCell(table, dict1[item.RepairType], item.RepairCount.ToString());
                }
                document.Add(table);
            }
        }
        private void AddOthersTablesCell(PdfPTable table, string h1, string h2)
        {
            table.AddCell(new PdfPCell(new Phrase(h1)));
            table.AddCell(new PdfPCell(new Phrase(h2)));
        }

        private void AddFactTable(Document document)
        {
            document.NewPage();
            document.Add(new Paragraph(new Phrase { new Chunk(("All repairs table").ToUpper()) })
            {
                Alignment = Element.ALIGN_CENTER,
            });
            var table = new PdfPTable(9)
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
            };
            
            AddHeaderRow(table);
            foreach (var item in _dbRepository.GetRepairServicesFacts())
            {
                AddContentRow(table, item);
            }
            document.Add(table);
        }
        private void AddGeneralPage(Document document)
        {
            document.Add(new Paragraph(new Phrase { new Chunk(("General repair firm analysis report").ToUpper()) })
            {
                Alignment = Element.ALIGN_CENTER,
            });
            document.Add(new Paragraph(new Phrase { new Chunk("On the first page you can find general information about this report file.") })
            {
                Alignment = Element.ALIGN_JUSTIFIED
            });
            document.Add(new Paragraph(new Phrase { new Chunk("From the second page to seventh you can find fact table with all main data in Repair Firm Storage.") })
            {
                Alignment = Element.ALIGN_JUSTIFIED
            });
            document.Add(new Paragraph(new Phrase { new Chunk("From the eighth to the eleventh page you can find simplified data tables for all three business tasks.") })
            {
                Alignment = Element.ALIGN_JUSTIFIED
            });
            document.Add(new Paragraph(new Phrase { new Chunk("From the twelth to eighteenth page you can find diagrams for business analysis task 1.") })
            {
                Alignment = Element.ALIGN_JUSTIFIED
            });
            document.Add(new Paragraph(new Phrase { new Chunk("From the nineteenth to twentieth page you can find diagrams for business analysis task 2.") })
            {
                Alignment = Element.ALIGN_JUSTIFIED
            });
            document.Add(new Paragraph(new Phrase { new Chunk("On the twenty one page you can find diagram for business analysis task 3.") })
            {
                Alignment = Element.ALIGN_JUSTIFIED
            });
        }

        private void AddContentRow(PdfPTable table, RepairServicesFactModel repair)
        {
            table.AddCell(new PdfPCell(new Phrase(repair.RepairStartDate.ToString("yyyy-MM-dd"))));
            table.AddCell(new PdfPCell(new Phrase(repair.RepairEndDate.ToString("yyyy-MM-dd"))));
            table.AddCell(new PdfPCell(new Phrase(ukrToEngDictionary[repair.RepairName])));
            table.AddCell(new PdfPCell(new Phrase(repair.RepairCount.ToString())));
            table.AddCell(new PdfPCell(new Phrase(repair.RepairTotalHours.ToString())));
            table.AddCell(new PdfPCell(new Phrase(repair.RepairServiceTotalPrice.ToString())));
            table.AddCell(new PdfPCell(new Phrase(repair.RelationToTotalContractHours.ToString())));
            table.AddCell(new PdfPCell(new Phrase(repair.RelationToTotalContractPrice.ToString())));
            table.AddCell(new PdfPCell(new Phrase(repair.EmployeeCount.ToString())));

        }

        private void AddHeaderRow(PdfPTable table)
        {
            table.AddCell(new PdfPCell(new Phrase("Start Date")));
            table.AddCell(new PdfPCell(new Phrase("End Date")));
            table.AddCell(new PdfPCell(new Phrase("Name")));
            table.AddCell(new PdfPCell(new Phrase("Repair Count")));
            table.AddCell(new PdfPCell(new Phrase("Total Hours")));
            table.AddCell(new PdfPCell(new Phrase("Service Total Price")));
            table.AddCell(new PdfPCell(new Phrase("Relation To Total Contract Hours")));
            table.AddCell(new PdfPCell(new Phrase("Relation To Total Contract Price")));
            table.AddCell(new PdfPCell(new Phrase("Employee Count")));
        }

        private Bitmap GenerateImgForCity(string title, List<RepairsByCitiesData> data)
        {
            var plt = new ScottPlot.Plot(550, 250);            
            plt.Title(title);
            var pie = plt.AddPie(data.Select(x => Convert.ToDouble(x.RepairCount)).ToArray());
            pie.SliceLabels = data.Select(x => x.RepairType).ToArray();
            pie.ShowValues = true;
            plt.Legend();
            System.Drawing.Bitmap bmp = plt.Render();
            return bmp;
        }
        private Bitmap GenerateImgForContractPrice()
        {
            List<DataPoint> dataPoints1 = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();
            List<DataPoint> dataPoints3 = new List<DataPoint>();

            List<List<DataPoint>> points = new List<List<DataPoint>>();
            var temp = _dbRepository.GetDepartmentPayments().GroupBy(x => x.CityName);
            int i = 0, j = 1;
            List<string> cities = new List<string>();
            foreach (var group in temp)
            {
                points.Add(new List<DataPoint>());

                foreach (var item in group)
                {
                    points[i].Add(new DataPoint { Label = j.ToString(), Y = item.TotalPrice });
                    j++;
                }
                cities.Add(group.Key);
                j = 1;
                i++;
            }

            dataPoints1 = points[0];
            dataPoints2 = points[1];
            dataPoints3 = points[2];



            var plt = new ScottPlot.Plot(700, 400);

            double[] positions = DataGen.Consecutive(dataPoints2.Count);
            plt.AddScatter(positions, dataPoints2.Select(x => (double)x.Y).ToArray(), label: cities[1]);

            plt.XAxis.ManualTickPositions(dataPoints2.Select((x, i) => (double)(i)).ToArray(), dataPoints2.Select(x => x.Label + " контракт").ToArray());
            plt.YAxis.ManualTickPositions(dataPoints2.Select((x, i) => (double)(x.Y)).ToArray(),
                dataPoints2.Select(x => x.Y.ToString()).ToArray());

            positions = DataGen.Consecutive(dataPoints3.Count);
            plt.AddScatter(positions, dataPoints3.Select(x => (double)x.Y).ToArray(), label: cities[2]);

            plt.XAxis.ManualTickPositions(dataPoints3.Select((x, i) => (double)(i)).ToArray(), dataPoints3.Select(x => x.Label + " контракт").ToArray());
            plt.YAxis.ManualTickPositions(dataPoints3.Select((x, i) => (double)(x.Y)).ToArray(),
                dataPoints3.Select(x => x.Y.ToString()).ToArray());

            positions = DataGen.Consecutive(dataPoints1.Count);
            plt.AddScatter(positions, dataPoints1.Select(x => (double)x.Y).ToArray(), label: cities[0]);

            plt.XAxis.ManualTickPositions(dataPoints1.Select((x, i) => (double)(i)).ToArray(), dataPoints1.Select(x => x.Label + " контракт").ToArray());
            plt.YAxis.ManualTickPositions(dataPoints1.Select((x, i) => (double)(x.Y)).ToArray(),
                dataPoints1.Select(x => x.Y.ToString()).ToArray());

            var legend = plt.Legend(enable: true);
            legend.FontSize = 8;
            legend.Location = Alignment.UpperRight;



            System.Drawing.Bitmap bmp = plt.Render();
            return bmp;
        }

        private Bitmap GenerateImgForEmployeeCount()
        {
            var list = _dbRepository.GetEmployeeForRepairType();
            list.ForEach(x => x.RepairType = x.RepairType.Replace(' ', '\n'));

            var plt = new ScottPlot.Plot(700, 400);

            double[] positions = DataGen.Consecutive(list.Count);
            var splt = plt.AddScatter(positions, list.Select(x => Convert.ToDouble(x.EmpoyeeCount)).ToArray());
            plt.AddFill(positions, list.Select(x => Convert.ToDouble(x.EmpoyeeCount)).ToArray());
            plt.XAxis.ManualTickPositions(list.Select((x, i) => (double)(i)).ToArray(), list.Select(x => x.RepairType).ToArray());
            plt.YAxis.ManualTickPositions(list.Select((x, i) => (double)(x.EmpoyeeCount)).ToArray(),
                list.Select(x => x.EmpoyeeCount.ToString()).ToArray());
            

            System.Drawing.Bitmap bmp = plt.Render();
            return bmp;
        }

        private Bitmap GenerateImgForContractCounts()
        {
            var list = _dbRepository.GetDepartmentServices();
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 1)
                    list[i].CityName = "\n" + list[i].CityName;
            }

            var plt = new ScottPlot.Plot(700, 400);

            double[] positions = DataGen.Consecutive(list.Count);
            plt.AddScatter(positions, list.Select(x => Convert.ToDouble(x.ServicesCount)).ToArray());

            plt.XAxis.ManualTickPositions(list.Select((x, i) => (double)(i)).ToArray(), list.Select(x => x.CityName).ToArray());
            plt.YAxis.ManualTickPositions(list.Select((x, i) => (double)(x.ServicesCount)).ToArray(),
                list.Select(x => x.ServicesCount.ToString()).ToArray());


            System.Drawing.Bitmap bmp = plt.Render();
            return bmp;
        }

        private Bitmap GenerateImgForRepairsPopular()
        {
            var list = _dbRepository.GetRepairCountChart();
        list.ForEach(x => x.RepairType = x.RepairType.Replace(' ', '\n'));

            var plt = new ScottPlot.Plot(700, 400);

            double[] positions = DataGen.Consecutive(list.Count);
            plt.AddBar(list.Select(x => Convert.ToDouble(x.RepairCount)).ToArray(), positions);

            plt.XAxis.ManualTickPositions(list.Select((x, i) => (double)(i)).ToArray(), list.Select(x => x.RepairType).ToArray());
            plt.YAxis.ManualTickPositions(list.Select((x, i) => (double)(x.RepairCount)).ToArray(),
                list.Select(x => x.RepairCount.ToString()).ToArray());


            System.Drawing.Bitmap bmp = plt.Render();
            return bmp;
        }

        private void AddDiagram(Document document, string name, Bitmap bmp)
        {
            document.NewPage();
            document.Add(new Paragraph(new Phrase { new Chunk((name).ToUpper()) })
            {
                Alignment = Element.ALIGN_CENTER,
            });
            using (var stream2 = new MemoryStream())
            {
                bmp.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
                stream2.Position = 0;
                var png = iTextSharp.text.Image.GetInstance(stream2);
                png.RotationDegrees = 270;
                png.Rotate();
                png.Alignment = Element.ALIGN_CENTER;
                document.Add(png);
            }
        }
        private void AddPie(Document document, Bitmap bmp, int newPage)
        {
            if (newPage == 0)
                document.NewPage();
            else
                document.Add(new PdfDiv()
                {
                    Bottom = 10.5f
                });

            using (var stream2 = new MemoryStream())
            {
                bmp.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
                stream2.Position = 0;
                var png = iTextSharp.text.Image.GetInstance(stream2);
                png.Alignment = Element.ALIGN_CENTER;
                document.Add(png);
            }
        }
        Dictionary<string, string> cities = new Dictionary<string, string>()
{
    {"Київ", "Kyiv"},
    {"Харків", "Kharkiv"},
    {"Одеса", "Odesa"},
    {"Дніпро", "Dnipro"},
    {"Бердянськ", "Berdyansk"},
    {"Запоріжжя", "Zaporizhzhia"},
    {"Кривий Ріг", "Kryvyi Rih"},
    {"Миколаїв", "Mykolaiv"},
    {"Маріуполь", "Mariupol"},
    {"Херсон", "Kherson"},
    {"Чернігів", "Chernihiv"},
    {"Полтава", "Poltava"},
    {"Черкаси", "Cherkasy"},
    {"Суми", "Sumy"},
    {"Житомир", "Zhytomyr"},
    {"Північний Братослав", "North Bratoslava"}
};
        Dictionary<string, string> ukrToEngDictionary = new Dictionary<string, string>()
{
    {"Ремонт кухні", "Kitchen renovation"},
    {"Ремонт спальні", "Bedroom renovation"},
    {"Ремонт дитячої кімнати", "Children's room renovation"},
    {"Ремонт вітальні", "Living room renovation"},
    {"Ремонт офісу", "Office renovation"},
    {"Ремонт балкону", "Balcony renovation"},
    {"Ремонт ванної кімнати", "Bathroom renovation"},
    {"Косметичний ремонт кухні", "Cosmetic kitchen renovation"},
    {"Капітальний ремонт квартири", "Capital apartment renovation"},
    {"Косметичний ремонт ванної кімнати", "Cosmetic bathroom renovation"},
    {"Ремонт стелі в головній спальні", "Ceiling renovation in the master bedroom"},
    {"Ремонт підлоги в гостьовій кімнаті", "Guest room floor renovation"},
    {"Ремонт кухонного блоку", "Kitchen unit renovation"},
    {"Стандартний ремонт віконних рам", "Standard window frame repair"},
    {"Мінімальний ремонт сходів", "Minimal stair repair"},
    {"Мінімальний ремонт віконних рам", "Minimal window frame repair"},
    {"Ремонт кімнати в стилі хай-тек", "High-tech room renovation"},
    {"Ремонт балкона", "Balcony renovation"},
    {"Монтаж відеодомофону", "Video intercom installation"},
    {"Встановлення бойлера", "Boiler installation"},
    {"Ремонт замку на вхідних дверях", "Entrance door lock repair"},
    {"Встановлення світильників", "Lamp installation"},
    {"Монтаж кондиціонера", "Air conditioner installation"},
    {"Встановлення нової системи опалення", "New heating system installation"},
    {"Ремонт водопровідної системи на кухні", "Kitchen plumbing system repair"},
    {"Поклейка нового шпалерного покриття в спальні", "New wallpaper installation in the bedroom"},
    {"Заміна старих розеток на нові на всій квартирі", "Replacing old sockets with new ones throughout the apartment"}
};
        Dictionary<string, string> dict1 = new Dictionary<string, string>()
{
    {"Сантехнічний ремонт", "Plumbing repair"},
    {"Під ключ", "Turnkey"},
    {"Заміна вікон", "Window replacement"},
    {"Ремонт підлоги", "Floor repair"},
    {"Малювання стін", "Wall painting"},
    {"Ванна кімната", "Bathroom"},
    {"Кухня", "Kitchen"},
    {"Євроремонт", "Euro-repair"},
    {"Балкон", "Balcony"},
    {"Коридор", "Hallway"}
};

    }
}
