using Microsoft.AspNetCore.Mvc;

namespace EasyBuy.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFoundPage()
        {
            return View();
        }
        public IActionResult Error404()
        {
            return View();
        }
    }
}
