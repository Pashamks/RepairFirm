using Microsoft.AspNetCore.Mvc;

namespace RepairFirm.Controllers
{
    [ApiController]
    [Route("")]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
