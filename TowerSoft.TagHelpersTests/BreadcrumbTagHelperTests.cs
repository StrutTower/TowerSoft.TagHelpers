using Microsoft.AspNetCore.Razor.TagHelpers;
using TowerSoft.TagHelpers;
using TowerSoft.TagHelpersTests.Utilities;

namespace TowerSoft.TagHelpersTests {
    [TestClass]
    public class BreadcrumbTagHelperTests {
        [TestMethod]
        public void Test() {
            BreadcrumbTagHelper breadcrumbTagHelper = new();

            TagHelperOutput output = TagHelperUtils.GetOutput();
            breadcrumbTagHelper.Process(TagHelperUtils.GetContext(), output);

            Assert.AreEqual("ul", output.TagName);
            Assert.IsTrue(output.Attributes["class"].Value.ToString().Contains("breadcrumb"));
        }
    }
}
