using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
