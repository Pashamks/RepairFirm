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

            if (System.IO.File.Exists("new.pdf"))
            {
                return View();
            }

            
            var list2 = _dbRepository.GetDepartmentServices();


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

            var chart4 = _dbRepository.GetEmployeeForRepairType();
            Bitmap bmp = GenerateImgForRepairsPopular();

            var stream = new FileStream("new.pdf", FileMode.CreateNew);

            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
            document.AddAuthor("Pavlo Somko");
            document.AddTitle("Repair firm analysis report");

            AddPopularRepairDiagram(bmp, document);

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

        private static void AddPopularRepairDiagram(Bitmap bmp, Document document)
        {
            document.NewPage();
            document.Add(new Paragraph(new Phrase { new Chunk(("The most popular repairs diagram").ToUpper()) })
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

        [HttpPost]
        [Route("GeneratePdf")]
        public IActionResult GeneratePdf()
        {
            return View();
        }

    }
}
