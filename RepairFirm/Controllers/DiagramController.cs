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



			ViewData["chart"] = JsonConvert.SerializeObject(list);

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
