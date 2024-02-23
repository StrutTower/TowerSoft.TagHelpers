using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Policy;

namespace DemoSite.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger) {
            _logger = logger;
        }

        [Required]
        [Display(Name = "Test String")]
        [Description("This is the description for the TestString property")]
        public string TestString { get; set; }

        public bool TestBooleanTrue { get; set; }

        public bool TestBooleanFalse { get; set; }

        public bool? TestBooleanNullable { get; set; }

        public bool? TestBooleanNullableTrue { get; set; }

        public bool? TestBooleanNullableFalse { get; set; }

        public DateTime TestDate { get; set; }

        public IFormFile TestFile { get; set; }

        public int TestInt { get; set; }

        public long TestLong { get; set; }

        public SelectList TestSelectList { get; set; }

        public byte[] ImageData { get; set; }

        public string NullString { get; set; }

        public string BlankString { get; set; }

        public int? NullInt { get; set; }

        public void OnGet() {
            TestString = "Model String";
            TestBooleanTrue = true;
            TestBooleanFalse = false;
            TestBooleanNullable = null;
            TestBooleanNullableTrue = true;
            TestBooleanNullableFalse = false;
            TestDate = DateTime.Now;
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "towersoft-logo_256x.png");
            ImageData = System.IO.File.ReadAllBytes(imagePath);
            TestSelectList = new SelectList(new[] { "Item 1", "Item 2", "Item 3" });
            BlankString = string.Empty;
        }
    }
}