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
        public HomeController(RepairDbContext repairDbContext)
        {
            _repairDbContext = repairDbContext;
        }
        public IActionResult Index()
        {
            var data = new HomeData();
            data.IsConnected = _repairDbContext.Database.CanConnect();
            return View(data);
        }
    }
}
