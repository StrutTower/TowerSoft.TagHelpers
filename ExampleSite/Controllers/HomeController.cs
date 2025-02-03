using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ExampleSite.Controllers {
    public class HomeController() : Controller {
        public IActionResult Index() {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "towersoft-logo_128x.png");
            IndexModel model = new() {
                ImageData = System.IO.File.ReadAllBytes(imagePath),
            };

            List<TestIntObject> testObjects = [];
            for (int i = 1; i < 7; i++) {
                testObjects.Add(new() { ID = i, Name = "Test Object " + i });
            }
            model.FormSelectIntList = new SelectList(testObjects, "ID", "Name");


            List<TestLongObject> testLongObjects = [];
            for (int i = 1; i < 7; i++) {
                testLongObjects.Add(new() { ID = i, Name = "Test Object " + i });
            }
            model.FormSelectLongList = new SelectList(testLongObjects, "ID", "Name");

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
