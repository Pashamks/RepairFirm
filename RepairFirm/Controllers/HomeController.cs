using EfCoreRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RepairFirm.Shared.Models;

namespace RepairFirm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly RepairDbContext _repairDbContext;
        private readonly IDbRepository _dbRepository;
        public HomeController(RepairDbContext repairDbContext, IDbRepository dbRepository)
        {
            _repairDbContext = repairDbContext;
            _dbRepository = dbRepository;
        }
        public IActionResult Index()
        {
            var data = new HomeData();
            data.IsConnected = _repairDbContext.Database.CanConnect();
            GenerateDonats();

            data.RepairCountChartDatas = _dbRepository.GetRepairCountChart();
            data.DepartmentContractDatas = _dbRepository.GetDepartmentServices();
            data.EmployeeForRepairDatas = _dbRepository.GetEmployeeForRepairType();

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
            data.RepairsPriceForContractsDatas = points;

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
      
            data.RepairsByCitiesDatas = repairsByCitiesDatas;
            return View(data);
        }
        [HttpGet]
        [Route("Perv")]
        public IActionResult Perv()
        {
            _dbRepository.PervLoading();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Increment")]
        public IActionResult Increment()
        {
            _dbRepository.IncrementLoading();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("Clear")]
        public IActionResult Clear()
        {
            _dbRepository.ClearStorage();
            return RedirectToAction("Index", "Home");
        }
        private void GenerateDonats()
        {
            var donats = _dbRepository.GetRepairsByCity().GroupBy(x => x.CityName);
            List<List<RepairsByCitiesData>> list = new List<List<RepairsByCitiesData>>();
            int i = 0;
            foreach (var group in donats)
            {
                list.Add(new List<RepairsByCitiesData>());
                ViewData[$"donat{i}"] = group.Key;
                foreach (var item in group)
                {
                    list[i].Add(item);
                }
                i++;
            }
            for (int j = 0; j < list.Count; j++)
            {
                var dist = list[j].DistinctBy(x => x.RepairType).ToList();
                foreach (var d in dist)
                {
                    d.RepairCount = list[j].Where(x => x.RepairType == d.RepairType).Sum(x => x.RepairCount);
                }
                list[j] = dist;
            }
            List<List<DataPoint>> points = list.Select(x => x.Select(x => new DataPoint
            {
                Label = x.RepairType,
                Y = x.RepairCount
            }).ToList()).ToList();

            for (int j = 0; j < points.Count; j++)
            {
                ViewData["donatdata" + j] = JsonConvert.SerializeObject(points[j]);
            }
        }

       
    }
}
