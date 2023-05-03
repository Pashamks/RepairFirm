using EfCoreRepository;
using Microsoft.AspNetCore.Mvc;

namespace RepairFirm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private readonly IDbRepository _dbRepository;
        public StorageController(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public IActionResult Index()
        {
            return View(_dbRepository.GetRepairServicesFacts());
        }
    }
}
