using Microsoft.AspNetCore.Mvc;

namespace Food.mvc.Controllers
{
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
