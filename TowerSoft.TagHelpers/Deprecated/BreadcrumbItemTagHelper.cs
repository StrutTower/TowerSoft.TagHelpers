using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.TagHelpers.Breadcrumbs;

namespace TowerSoft.TagHelpers.Deprecated {

    [Obsolete("Switch to breadcrumb")]
    [HtmlTargetElement("breadcrumb-item", ParentTag = "breadcrumbs")]
    public class BreadcrumbItemTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) : BreadcrumbTagHelper(htmlHelper, urlHelperFactory) {

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            await base.ProcessAsync(context, output);

            if (System.Diagnostics.Debugger.IsAttached) {
                string content = output.Content.GetContent();

                TagBuilder abbr = new("abbr");
                abbr.Attributes.Add("title", "The TagHelper breadcrumb-item is deprecated. Switch to breadcrumb.");
                abbr.InnerHtml.AppendHtml(content.Replace("<a", "&#9888;<a class=\"text-danger\""));
                output.Content.SetHtmlContent(abbr);
            }

        }
    }
}
