using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using TowerSoft.TagHelpers;
using TowerSoft.TagHelpersTests.Utilities;

namespace TowerSoft.TagHelpersTests {
    [TestClass]
    public class AnchorEnabledTagHelperTests {
        [TestMethod]
        public void DisabledLink() {
            AnchorEnabledTagHelper anchorEnabledTagHelper = new() {
                Enabled = false
            };
            TagHelperOutput output = TagHelperUtils.GetOutput("a");
            anchorEnabledTagHelper.Process(TagHelperUtils.GetContext("a"), output);

            Assert.AreEqual("a", output.TagName);
            Assert.IsNotNull(output.Attributes);
            Assert.IsTrue(output.Attributes.ContainsName("class"));
            Assert.IsTrue(output.Attributes["class"].Value.ToString().Contains("disable-link"));
        }

        [TestMethod]
        public void EnabledLink() {
            AnchorEnabledTagHelper anchorEnabledTagHelper = new() {
                Enabled = true
            };
            TagHelperOutput output = TagHelperUtils.GetOutput("a");
            anchorEnabledTagHelper.Process(TagHelperUtils.GetContext("a"), output);

            Assert.AreEqual("a", output.TagName);
            Assert.IsNotNull(output.Attributes);

            if (output.Attributes.ContainsName("class"))
                Assert.IsFalse(output.Attributes["class"].Value.ToString().Contains("disable-link"));
        }

        [TestMethod]
        public void EnabledLink_WithOtherClasses() {
            AnchorEnabledTagHelper anchorEnabledTagHelper = new() {
                Enabled = true
            };
            TagHelperOutput output = TagHelperUtils.GetOutput("a");
            output.AddClass("text-danger", HtmlEncoder.Default);


            anchorEnabledTagHelper.Process(TagHelperUtils.GetContext("a"), output);

            Assert.AreEqual("a", output.TagName);
            Assert.IsNotNull(output.Attributes);
            Assert.IsTrue(output.Attributes.ContainsName("class"));
            Assert.IsTrue(output.Attributes["class"].Value.ToString().Contains("text-danger"));
            Assert.IsFalse(output.Attributes["class"].Value.ToString().Contains("disable-link"));
        }
    }
}
