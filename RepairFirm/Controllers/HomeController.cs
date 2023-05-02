using Microsoft.AspNetCore.Mvc;

namespace RepairFirm.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
