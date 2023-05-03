using EfCoreRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace RepairFirm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiagramController : Controller
    {
		private readonly IDbRepository _dbRepository;
		public DiagramController(IDbRepository dbRepository)
        {
			_dbRepository = dbRepository;
        }
		public IActionResult Index()
		{
			var list = _dbRepository.GetRepairCountChart().Select(x => new DataPoint
			{
				Label = x.RepairType,
				Y = x.RepairCount
			}).ToList();

			var list2 = _dbRepository.GetDepartmentServices().Select(x => new DataPoint
			{
				Label = x.CityName,
				Y = x.ServicesCount
			}).ToList();
			ViewData["repairChart"] = JsonConvert.SerializeObject(list);
			ViewData["departmetnCountChart"] = JsonConvert.SerializeObject(list2);

			#region 3 Chart

			List<DataPoint> dataPoints1 = new List<DataPoint>();
			List<DataPoint> dataPoints2 = new List<DataPoint>();
			List<DataPoint> dataPoints3 = new List<DataPoint>();

			List<List<DataPoint>> points = new List<List<DataPoint>>();
			var temp = _dbRepository.GetDepartmentPayments().GroupBy(x => x.CityName);
			int i = 0, j= 1;
            foreach (var group in temp)
            {
				points.Add(new List<DataPoint>());

				foreach (var item in group)
                {
					points[i].Add(new DataPoint { Label = j.ToString(), Y = item.TotalPrice });
					j++;
                }
				ViewData["city"+(i+1)] = group.Key;
				j = 1;
				i++;
            }

			dataPoints1 = points[0];
			dataPoints2 = points[1];
			dataPoints3 = points[2];


			ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
			ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
			ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);
			#endregion

			var chart4 = _dbRepository.GetEmployeeForRepairType().Select(x => new DataPoint
			{
				Label = x.RepairType,
				Y = x.EmpoyeeCount
			}).ToList(); ;
			ViewData["chart4"] = JsonConvert.SerializeObject(chart4);

			return View();
		}
	}
	[DataContract]
	public class DataPoint
	{
        public DataPoint()
        {

        }
		public DataPoint(string label, double y)
		{
			this.Label = label;
			this.Y = y;
		}

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "label")]
		public string Label = "";

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;
	}
}
