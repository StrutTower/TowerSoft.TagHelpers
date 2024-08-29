using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ExampleSite.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "towersoft-logo_128x.png");
            IndexModel model = new() {
                TestString = "Model String",
                TestBooleanTrue = true,
                TestBooleanFalse = false,
                TestBooleanNullable = null,
                TestBooleanNullableTrue = true,
                TestBooleanNullableFalse = false,
                TestDate = DateTime.Now,
                ImageData = System.IO.File.ReadAllBytes(imagePath),
                TestSelectList = new SelectList(new[] { "Item 1", "Item 2", "Item 3" }),
                BlankString = string.Empty
            };
            return View(model);
        }

        public IActionResult Test() {
            return Content("Index controller, Test Action");
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
