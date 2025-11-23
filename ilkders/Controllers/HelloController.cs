using Microsoft.AspNetCore.Mvc;

namespace ilkders.Controllers
{
    public class HelloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
