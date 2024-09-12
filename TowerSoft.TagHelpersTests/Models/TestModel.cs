using System.ComponentModel;

namespace TowerSoft.TagHelpersTests.Models {
    public class TestModel {
        [System.ComponentModel.Description("test description")]
        public string PropertyWithDescription { get; set; }
    }
}
