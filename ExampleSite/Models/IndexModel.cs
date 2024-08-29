using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExampleSite.Models {
    public class IndexModel {
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
    }
}
