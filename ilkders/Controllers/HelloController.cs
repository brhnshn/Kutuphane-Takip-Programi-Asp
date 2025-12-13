using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class HelloController : Controller
    {
        public IActionResult Index()
        {
            return View();
     }
    }
}
