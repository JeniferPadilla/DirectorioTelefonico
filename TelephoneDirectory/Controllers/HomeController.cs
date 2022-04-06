using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TelephoneDirectory.Models;

namespace TelephoneDirectory.Controllers
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

          //  var message = "Hola prueba";

            //ViewData["Message"] = message;
            //  ViewBag.message = message;

            ViewBag.message = "Hola prueba";

            return View();
        }

        public IActionResult Privacy()
        {
         //   ViewBag.message = "hola";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}