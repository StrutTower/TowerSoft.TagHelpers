using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Options;

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
            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("aria-label", "breadcrumb");

            TagBuilder ul = new("ul");
            if (TowerSoftTagHelperSettings.BreadcrumbContainerClass != null)
                ul.AddCssClass(TowerSoftTagHelperSettings.BreadcrumbContainerClass);

            TagHelperContent childContent = output.GetChildContentAsync().Result;
            ul.InnerHtml.AppendHtml(childContent);

            output.Content.AppendHtml(ul);

            //output.TagName = "ul";
            //output.TagMode = TagMode.StartTagAndEndTag;
            //output.AddClass("breadcrumb", HtmlEncoder.Default);
        }
    }
}
