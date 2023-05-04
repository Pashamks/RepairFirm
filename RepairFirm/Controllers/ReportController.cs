using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using EfCoreRepository;
using ScottPlot;

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
            var list = _dbRepository.GetRepairCountChart();

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

            var plt = new ScottPlot.Plot(800, 400);

            double[] positions = DataGen.Consecutive(list.Count);
            plt.AddBar(list.Select(x => Convert.ToDouble(x.RepairCount)).ToArray(), positions);

            plt.XAxis.ManualTickPositions(list.Select((x, i) => (double)(i)).ToArray(), list.Select(x => x.RepairType).ToArray());
            plt.YAxis.ManualTickPositions(list.Select((x, i) => (double)(x.RepairCount)).ToArray(), 
                list.Select(x => x.RepairCount.ToString()).ToArray());

            //plt.YAxis.Ticks(false);
            //plt.YAxis.Label("Title");

            // vertical axis (when rotated)
           // plt.XAxis.Ticks(true);
            plt.XAxis.TickLabelStyle(rotation: 90);

            // horizontal axis (when rotated)
            //plt.YAxis2.Ticks(true);
            plt.YAxis2.TickLabelStyle(rotation: 90);
            //plt.YAxis.TickDensity(1);

            var img = plt.GetImageHTML();


            var ChromePdfRenderer = new ChromePdfRenderer();

            //// render html to pdf
            var PDF = ChromePdfRenderer.RenderHtmlAsPdf(img);
            //// output to pdf
            var OutputPath = "HtmlToPDF.pdf";
            PDF.SaveAs(OutputPath);
            //var list = _dbRepository.GetRepairCountChart();
            //var plt = new ScottPlot.Plot(400, 300);
            //plt.AddScatter(list.Select(x => x.RepairCount).ToArray(),list.Select(x => x.RepairCount).ToArray());

            return View();
        }
        [HttpPost]
        [Route("GeneratePdf")]
        public IActionResult GeneratePdf()
        {
            return View();
        }

    }
}
