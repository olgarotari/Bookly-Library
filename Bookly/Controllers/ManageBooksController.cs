using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class ManageBooksController : Controller
    {
        public IActionResult Index()
        {
            return View("/Views/Shared/_AdminLayout.cshtml");
        }
    }
}
