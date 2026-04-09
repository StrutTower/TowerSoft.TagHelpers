using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Options;

namespace TowerSoft.TagHelpers.TagHelpers.Forms {
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
            if (ForceRequiredAstrix.HasValue && ForceRequiredAstrix.Value ||
                !ForceRequiredAstrix.HasValue && For.Metadata.IsRequired) {

                TagBuilder span = new("span");
                span.InnerHtml.Append(" * ");
                if (TowerSoftTagHelperSettings.LabelRequiredAstrixClass != null)
                    span.AddCssClass(TowerSoftTagHelperSettings.LabelRequiredAstrixClass);
                span.Attributes.Add("title", "This field is required.");

                if (TowerSoftTagHelperSettings.LabelRequiredAstrixInLabel) {
                    output.Content.AppendHtml(span);
                } else {
                    output.PostElement.SetHtmlContent(span);
                }
            }
        }
    }
}
