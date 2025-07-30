using Microsoft.AspNetCore.Mvc;

namespace EasyBuy.Areas.NVKT.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
