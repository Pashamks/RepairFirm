using EfCoreRepository;
using Microsoft.AspNetCore.Mvc;
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
    }
}
