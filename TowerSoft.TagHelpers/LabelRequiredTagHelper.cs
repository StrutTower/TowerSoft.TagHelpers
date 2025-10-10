using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Extend the default LabelTagHelper. Adds a red astrix to the end if the field is required.
    /// </summary>
    /// <param name="htmlGenerator"></param>
    [HtmlTargetElement("label", Attributes = "asp-for")]
    public class LabelRequiredTagHelper(IHtmlGenerator htmlGenerator) : LabelTagHelper(htmlGenerator) {

        /// <summary>
        /// Allows manually setting the required value for the red astrix
        /// </summary>
        [HtmlAttributeName("asp-force-required")]
        public bool? ForceRequiredAstrix { get; set; }

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            await base.ProcessAsync(context, output);
            if ((ForceRequiredAstrix.HasValue && ForceRequiredAstrix.Value) ||
                (!ForceRequiredAstrix.HasValue && For.Metadata.IsRequired)) {

                TagBuilder span = new("span");
                span.InnerHtml.Append("*");
                span.AddCssClass("text-danger ps-1");
                span.Attributes.Add("title", "This field is required.");
                output.PostElement.SetHtmlContent(span);
            }
        }
    }
}
