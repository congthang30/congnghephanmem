using Microsoft.AspNetCore.Mvc;

namespace EasyBuy.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
