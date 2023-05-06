using EfCoreRepository;
using Microsoft.AspNetCore.Mvc;

namespace RepairFirm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetaController : Controller
    {
        private readonly IDbRepository _dbRepository;
        public MetaController(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public IActionResult Index()
        {       
            return View(_dbRepository.GetMetaData());
        }
    }
}
