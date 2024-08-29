using Microsoft.AspNetCore.Mvc;

namespace ExampleSite.Area.Reports.Controllers {
    [Area("Reports")]
    public class HomeController : Controller {
        public IActionResult Index() {
            return Content("Reports area placeholder");
        }
    }
}
