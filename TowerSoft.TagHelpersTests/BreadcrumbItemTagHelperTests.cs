using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.TagHelpers;
using TowerSoft.TagHelpersTests.Utilities;

namespace TowerSoft.TagHelpersTests {
    [TestClass]
    public class BreadcrumbItemTagHelperTests {
        private IHtmlHelper htmlHelper;
        private IUrlHelperFactory urlHelperFactory;

        [TestInitialize]
        public void Initialize() {
            htmlHelper = null;
            urlHelperFactory = null;
        }

        //[TestMethod]
        public void GenerateBreadcrumbItem() {
            BreadcrumbItemTagHelper breadcrumbItemTagHelper = new(htmlHelper, urlHelperFactory);

            TagHelperOutput output = TagHelperUtils.GetOutput("breadcrumb-item");
            breadcrumbItemTagHelper.Process(TagHelperUtils.GetContext("breadcrumb-item"), output);

            Assert.AreEqual("li", output.TagName);
            Assert.IsTrue(output.Attributes["class"].Value.ToString().Contains("breadcrumb"));
        }
    }
}
