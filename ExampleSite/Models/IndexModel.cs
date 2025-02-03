using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExampleSite.Models {
    public class IndexModel {
        public byte[] ImageData { get; set; }
        public string BlankString { get; set; } = string.Empty;
        public string TestString { get; set; } = "Test String";
        public string DisplayTestString { get; set; } = "Test String";
        public DateTime DisplayTestDate { get; set; } = DateTime.Now;
        public string TruncateTestString { get; set; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";

        [Description("This is the description for the TestDescription property")]
        public string TestDescription { get; set; }

        [Required, Display(Name = "Label Test with Display Attribute")]
        public string LabelTestWithDisplay { get; set; }

        [Display(Name = "Label Test with DisplayName Attribute")]
        public string LabelTestWithDisplayName { get; set; }


        public string TrDisplayTestString { get; set; } = "tr-display Test String";
        public DateTime TrDisplayTestDate { get; set; } = DateTime.Now;
        public string TrDisplayNullString { get; set; }
        public int? TrDisplayNullInt { get; set; }
        public bool? TrDisplayTestBooleanNullable { get; set; }
        public bool? TrDisplayTestBooleanNullableTrue { get; set; } = true;
        public bool? TrDisplayTestBooleanNullableFalse { get; set; } = false;


        public string FormFieldTestString1 { get; set; } = "FormField Test String 1";
        public string FormFieldTestString2 { get; set; } = "FormField Test String 2";
        public bool FormFieldTestBooleanTrue { get; set; } = true;
        public bool FormFieldTestBooleanFalse { get; set; } = false;
        public bool? FormFieldTestBooleanNullable { get; set; }


        public int FormSelectIntID { get; set; } = 4;
        public List<int> FormSelectMultipleIntID { get; set; } = [3, 4];
        public long FormSelectLongID { get; set; } = 3;
        public string FormSelectString { get; set; } = "Item 3";


        public string HrFormFieldTestString1 { get; set; } = "HrFormField Test String 1";
        public string HrFormFieldTestString2 { get; set; } = "HrFormField Test String 2";
        public bool HrFormFieldTestBooleanTrue { get; set; } = true;
        public bool HrFormFieldTestBooleanFalse { get; set; } = false;
        public bool? HrFormFieldTestBooleanNullable { get; set; } = null;


        public int HrFormSelectIntID { get; set; } = 3;
        public List<int> HrFormSelectMultipleIntID { get; set; } = [1, 4];
        public long HrFormSelectLongID { get; set; } = 3;
        public string HrFormSelectString { get; set; } = "Item 3";


        public SelectList FormSelectIntList { get; set; }
        public SelectList FormSelectLongList { get; set; }
        public SelectList FormSelectStringList { get; set; } = new SelectList(items);


        public string HrFormContainerString { get; set; } = "Test String";
        public string HrFormContainerSelect { get; set; } = "value2";



        public bool RendererTestBooleanTrue { get; set; } = true;

        public bool RendererTestBooleanFalse { get; set; } = false;

        public bool? RendererTestBooleanNullable { get; set; } = null;

        public bool? RendererTestBooleanNullableTrue { get; set; } = true;

        public bool? RendererTestBooleanNullableFalse { get; set; } = false;

        public bool RendererTestBooleanTrue2 { get; set; } = true;

        public bool RendererTestBooleanFalse2 { get; set; } = false;

        public bool? RendererTestBooleanNullable2 { get; set; }

        public bool? RendererTestBooleanNullableTrue2 { get; set; } = true;

        public bool? RendererTestBooleanNullableFalse2 { get; set; } = false;

        public DateTime RendererTestDate1 { get; set; } = DateTime.Now;
        public DateTime RendererTestDate2 { get; set; } = DateTime.Now;
        public DateTime RendererTestDate3 { get; set; } = DateTime.Now;
        public IFormFile RendererTestFile { get; set; }
        public int RendererTestInt { get; set; } = 20;
        public long RendererTestLong { get; set; } = 50;
        public string RendererTestString1 { get; set; } = "test";
        public string RendererTestString2 { get; set; } = "test2";



        private static readonly string[] items = ["Item 1", "Item 2", "Item 3", "Item 4", "Item 5"];
    }
}
