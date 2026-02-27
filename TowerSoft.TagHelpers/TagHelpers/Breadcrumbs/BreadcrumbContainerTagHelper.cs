using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace TowerSoft.TagHelpers.TagHelpers.Breadcrumbs {
    /// <summary>
    /// Bootstrap breadcrumb container. Use breadcrumb-item as child elements to define the breadcrumb items.
    /// </summary>
    [HtmlTargetElement("breadcrumbs")]
    [RestrictChildren("breadcrumb", "breadcrumb-home", "breadcrumb-item", "breadcrumb-item-home")]
    public class BreadcrumbContainerTagHelper : TagHelper {

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "ul";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("breadcrumb", HtmlEncoder.Default);
        }
    }
}
