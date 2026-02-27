using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.TagHelpers.Breadcrumbs;

namespace TowerSoft.TagHelpers {
    [Obsolete("Switch to breadcrumb-home")]
    [HtmlTargetElement("breadcrumb-item-home", ParentTag = "breadcrumbs")]
    public class BreadcrumbItemHomeTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) : BreadcrumbHomeTagHelper(htmlHelper, urlHelperFactory) {

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            await base.ProcessAsync(context, output);

            if (System.Diagnostics.Debugger.IsAttached) {
                string content = output.Content.GetContent();

                TagBuilder abbr = new("abbr");
                abbr.Attributes.Add("title", "The TagHelper breadcrumb-item-home is deprecated. Switch to breadcrumb-home.");
                abbr.InnerHtml.AppendHtml(content.Replace("<a", "&#9888;<a class=\"text-danger\""));
                output.Content.SetHtmlContent(abbr);
            }

        }
    }
}
