using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
