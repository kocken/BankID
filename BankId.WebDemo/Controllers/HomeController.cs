using BankId.WebDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankId.WebDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new PersonalNumberModel());
        }

        [Route("about")]
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}