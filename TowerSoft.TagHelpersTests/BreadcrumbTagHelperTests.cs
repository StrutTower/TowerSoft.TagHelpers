using Microsoft.AspNetCore.Razor.TagHelpers;
using TowerSoft.TagHelpers.TagHelpers.Breadcrumbs;
using TowerSoft.TagHelpersTests.Utilities;

namespace TowerSoft.TagHelpersTests {
    [TestClass]
    public class BreadcrumbTagHelperTests {
        [TestMethod]
        public void GenerateBreadcrumbs() {
            BreadcrumbContainerTagHelper breadcrumbTagHelper = new();

            TagHelperOutput output = TagHelperUtils.GetOutput("breadcrumbs");
            breadcrumbTagHelper.Process(TagHelperUtils.GetContext("breadcrumbs"), output);

            Assert.AreEqual("ul", output.TagName);
            Assert.IsTrue(output.Attributes["class"].Value.ToString().Contains("breadcrumb"));
        }
    }
}
