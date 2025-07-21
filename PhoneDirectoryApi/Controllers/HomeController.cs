using Microsoft.AspNetCore.Mvc;

namespace PhoneDirectoryApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
