using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class ErrorController : Controller
    {
        // Hata olduğunda bu çalışacak
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Aradığınız sayfa veya kaynak bulunamadı.";
                    ViewBag.ErrorCode = "404";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sunucu tarafında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                    ViewBag.ErrorCode = "500";
                    break;
                default:
                    ViewBag.ErrorMessage = "Beklenmedik bir hata oluştu.";
                    ViewBag.ErrorCode = statusCode.ToString();
                    break;
            }

            return View("Error");
        }
    }
}