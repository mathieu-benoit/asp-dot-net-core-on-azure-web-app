using AspNetCoreApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = _homeService.About;

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = _homeService.Contact;

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
