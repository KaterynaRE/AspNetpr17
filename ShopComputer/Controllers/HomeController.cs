using Microsoft.AspNetCore.Mvc;

namespace ShopComputer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
