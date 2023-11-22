using Filters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Filters.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        [CustomActionFilter]
        public IActionResult Privacy()
        {
            ViewBag.Privacy = "Sensitive Information";
            return View();
        }
        [CustomExceptionFilter]
        public ActionResult Index()
        {
            int x = 10;
            int y = 0;
            int z = x / y;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}