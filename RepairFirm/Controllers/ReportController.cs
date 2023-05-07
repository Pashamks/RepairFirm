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

            if (System.IO.File.Exists("report.pdf"))
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
            

            var stream = new FileStream("report.pdf", FileMode.CreateNew);

            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
            document.AddAuthor("Pavlo Somko");
            document.AddTitle("Repair firm analysis report");

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


            var table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
            };

            //table.AddCell(new PdfPCell(pdfImage)
            //{
            //    Border = 0
            //});
            document.Add(table);
            document.NewPage();
            document.Add(new Paragraph(new Phrase { new Chunk(("General table").ToUpper()) })
            {
                Alignment = Element.ALIGN_CENTER,
            });

            document.Close();
            writer.Close();


            return View();
        }
        private Bitmap GenerateImgForCity(string title, List<RepairsByCitiesData> data)
        {
            var plt = new ScottPlot.Plot(550, 250);
            //plt.ManualDataArea(new PixelPadding(30,30,30,30));
            
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
            //document.Add(new Paragraph(new Phrase { new Chunk((name).ToUpper()) })
            //{
            //    Alignment = Element.ALIGN_CENTER,
            //});
            using (var stream2 = new MemoryStream())
            {
                bmp.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
                stream2.Position = 0;
                var png = iTextSharp.text.Image.GetInstance(stream2);
                png.Alignment = Element.ALIGN_CENTER;
                document.Add(png);
            }
        }
    }
}
