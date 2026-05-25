using System.Diagnostics;
using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KiranaStoreUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("health")]
        public IActionResult Health()
        {
            return Content("UI Running");
        }
    }
}
