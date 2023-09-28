using Microsoft.AspNetCore.Mvc;

namespace pet_store_noamcelermajer.Controllers
{
    public class AnimalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
