using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Bootstrap breadcrumb container. Use breadcrumb-item as child elements to define the breadcrumb items.
    /// </summary>
    [HtmlTargetElement("breadcrumbs")]
    [RestrictChildren("breadcrumb-item")]
    public class BreadcrumbTagHelper : TagHelper {

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "ul";
            output.AddClass("breadcrumb", HtmlEncoder.Default);
        }
    }
}
