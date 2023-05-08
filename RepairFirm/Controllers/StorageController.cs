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
            if(list == null)
            {
                var result = _dbRepository.GetRepairServicesFacts();
                FillViewBagLists(result);
                list = result;
                return View(result);
            }
            else
            {
                var result = list;
                FillViewBagLists(result);
                return View(list);
            }
            
        }
        [HttpPost]
        [Route("Reload")]
        public IActionResult Reload()
        {
            var result = _dbRepository.GetRepairServicesFacts();
            list = result;
            return RedirectToAction("Index", "Storage"); ;

        }
        private void FillViewBagLists(List<RepairServicesFactModel> result)
        {
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
        }
        [HttpPost]
        [Route("Filter")]
        public IActionResult Filter([FromForm]FilterRepairServicesFactModel model)
        {
            var result = list;
            if(model.IsMinEmployeeCount == "on")
                result = result.Where(x => model.MinEmployeeCount == null ? true : x.EmployeeCount > model.MinEmployeeCount)
                    .Where(x => model.MaxEmployeeCount == null ? true : x.EmployeeCount < model.MaxEmployeeCount).ToList();
            if(model.IsMinRepairCount == "on")
                result = result.Where(x => model.MinRepairCount == null ? true : x.RepairCount > model.MinRepairCount)
                    .Where(x => model.MaxRepairCount == null ? true : x.RepairCount < model.MaxRepairCount).ToList();
            if(model.IsMinRepairEndDate == "on")
                result = result.Where(x => model.MinRepairEndDate == null ? true : x.RepairEndDate > model.MinRepairEndDate)
                    .Where(x => model.MaxRepairEndDate == null ? true : x.RepairEndDate < model.MaxRepairEndDate).ToList();
            if(model.IsMinRepairStartDate == "on")
                result = result.Where(x => model.MinRepairStartDate == null ? true : x.RepairStartDate > DateTime.Parse(model.MinRepairStartDate))
                    .Where(x => model.MaxRepairStartDate == null ? true : x.RepairStartDate < model.MaxRepairStartDate).ToList();
            if(model.IsRepairName == "on")
                result = result.Where(x => model.RepairName == null ? true : model.RepairName.Contains(x.RepairName)).ToList();
            if(model.IsMinRelationToTotalContractHours == "on")
                result = result.Where(x => model.MinRelationToTotalContractHours == null ? true : x.RelationToTotalContractHours > model.MinRelationToTotalContractHours)
                    .Where(x => model.MaxRelationToTotalContractHours == null ? true : x.RelationToTotalContractHours < model.MaxRelationToTotalContractHours).ToList();
            if(model.IsMinRelationToTotalContractPrice == "on")
                result = result.Where(x => model.MinRelationToTotalContractPrice == null ? true : x.RelationToTotalContractPrice > model.MinRelationToTotalContractPrice)
                    .Where(x => model.MaxRelationToTotalContractPrice == null ? true : x.RelationToTotalContractPrice > model.MaxRelationToTotalContractPrice).ToList();
            if(model.IsMinRepairServiceTotalPrice == "on")
                result = result.Where(x => model.MinRepairServiceTotalPrice == null ? true : x.RepairServiceTotalPrice > model.MinRepairServiceTotalPrice)
                    .Where(x => model.MaxRepairServiceTotalPrice == null ? true : x.RepairServiceTotalPrice < model.MaxRepairServiceTotalPrice).ToList();
            if (model.IsMinRepairTotalHours == "on")
                result = result.Where(x => model.MinRepairTotalHours == null ? true : x.RepairTotalHours > model.MinRepairTotalHours)
                    .Where(x => model.MaxRepairTotalHours == null ? true : x.RepairTotalHours < model.MaxRepairTotalHours).ToList();

            list = result;
            return RedirectToAction("Index", "Storage"); ;
        }
    }
}
