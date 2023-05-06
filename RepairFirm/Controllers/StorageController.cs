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
                result = result.Where(x => x.EmployeeCount > model.MinEmployeeCount).ToList();
            if(model.IsMinRepairCount == "on")
                result = result.Where(x => x.RepairCount > model.MinRepairCount).ToList();
            if(model.IsMinRepairEndDate == "on")
                result = result.Where(x => x.RepairEndDate > model.MinRepairEndDate).ToList();
            if(model.IsMinRepairStartDate == "on")
                result = result.Where(x => x.RepairStartDate > DateTime.Parse(model.MinRepairStartDate)).ToList();
            if(model.IsContractId == "on")
                result = result.Where(x => x.ContractId == model.ContractId).ToList();
            if(model.IsRoomId == "on")
                result = result.Where(x => x.RoomId == model.RoomId).ToList();
            if(model.IsRepairName == "on")
                result = result.Where(x => x.RepairName == model.RepairName).ToList();
            if(model.IsMinRelationToTotalContractHours == "on")
                result = result.Where(x => x.RelationToTotalContractHours > model.MinRelationToTotalContractHours).ToList();
            if(model.IsMinRelationToTotalContractPrice == "on")
                result = result.Where(x => x.RelationToTotalContractPrice > model.MinRelationToTotalContractPrice).ToList();
            if(model.IsMinRepairServiceTotalPrice == "on")
                result = result.Where(x => x.RepairServiceTotalPrice > model.MinRepairServiceTotalPrice).ToList();
            if (model.IsMinRepairServiceTotalPrice == "on")
                result = result.Where(x => x.RepairServiceTotalPrice > model.MinRepairServiceTotalPrice).ToList();
            if (model.IsMinRepairTotalHours == "on")
                result = result.Where(x => x.RepairTotalHours > model.MinRepairTotalHours).ToList();

            if(model.IsMaxEmployeeCount == "on")
                result = result.Where(x => x.EmployeeCount < model.MaxEmployeeCount).ToList();
            if(model.IsMaxRepairCount == "on")
                result = result.Where(x => x.RepairCount < model.MaxRepairCount).ToList();
            if(model.IsMaxRepairEndDate == "on")
                result = result.Where(x => x.RepairEndDate < model.MaxRepairEndDate).ToList();
            if(model.IsMaxRepairStartDate == "on")
                result = result.Where(x => x.RepairStartDate < model.MaxRepairStartDate).ToList();
            if(model.IsMaxRelationToTotalContractHours == "on")
                result = result.Where(x => x.RelationToTotalContractHours < model.MaxRelationToTotalContractHours).ToList();
            if(model.IsMaxRelationToTotalContractPrice == "on")
                result = result.Where(x => x.RelationToTotalContractPrice < model.MaxRelationToTotalContractPrice).ToList();
            if(model.IsMaxRepairServiceTotalPrice == "on")
                result = result.Where(x => x.RepairServiceTotalPrice < model.MaxRepairServiceTotalPrice).ToList();
            if (model.IsMaxRepairServiceTotalPrice == "on")
                result = result.Where(x => x.RepairServiceTotalPrice < model.MaxRepairServiceTotalPrice).ToList();
            if (model.IsMaxRepairTotalHours == "on")
                result = result.Where(x => x.RepairTotalHours < model.MaxRepairTotalHours).ToList();
            list = result;
            return RedirectToAction("Index", "Storage"); ;
        }
    }
}
