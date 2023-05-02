using Microsoft.AspNetCore.Mvc;
using RepairFirm.Shared.Models;

namespace RepairFirm.Controllers
{
    [ApiController]
    [Route("")]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly LoginData _data;
        public LoginController(LoginData data)
        {
            _data = data;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromForm]LoginData data)
        {
            if(_data.Email == data.Email && _data.Password == _data.Password)
                return View();
            return View();
        }
    }
}
