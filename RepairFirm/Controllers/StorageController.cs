using EfCoreRepository;
using Microsoft.AspNetCore.Mvc;
using RepairFirm.Shared.Models;

namespace RepairFirm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private readonly IDbRepository _dbRepository;
        private static List<RepairServicesFactModel> list;
        public StorageController(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public IActionResult Index()
        {
            var result = _dbRepository.GetRepairServicesFacts();
            ViewBag.RepairList = result.Select(x => x.RepairName).Distinct().ToList();
            ViewBag.EmployeeCountList = result.Select(x => x.EmployeeCount).Distinct().ToList();
            ViewBag.RoomIdList = result.Select(x => x.RoomId).Distinct().ToList();
            ViewBag.ContractIdList = result.Select(x => x.ContractId).Distinct().ToList();
            ViewBag.RepairCountList = result.Select(x => x.RepairCount).Distinct().ToList();
            ViewBag.RepairTotalHoursList = result.Select(x => x.RepairTotalHours).Distinct().ToList();
            ViewBag.RepairServiceTotalPriceList = result.Select(x => x.RepairServiceTotalPrice).Distinct().ToList();
            ViewBag.RelationToTotalContractHoursList = result.Select(x => x.RelationToTotalContractHours).Distinct().ToList();
            ViewBag.RelationToTotalContractPriceList = result.Select(x => x.RelationToTotalContractPrice).Distinct().ToList();
            ViewBag.RepairStartDateList = result.Select(x => x.RepairStartDate).Distinct().ToList();
            ViewBag.RepairEndDateList = result.Select(x => x.RepairEndDate).Distinct().ToList();
            list = result;
            return View(result);
        }
        [HttpPost]
        [Route("Filter")]
        public IActionResult Filter([FromForm]RepairServicesFactModel model)
        {
            
            return View();
        }
    }
}
