using Microsoft.AspNetCore.Mvc;

namespace ExampleSite.Controllers {
    public class PlaceholderController : Controller {
        public IActionResult Index() {
            return Content("Placeholder controller index action");
        }
        public IActionResult View() {
            return Content("Placeholder controller view action");
        }
    }
}
