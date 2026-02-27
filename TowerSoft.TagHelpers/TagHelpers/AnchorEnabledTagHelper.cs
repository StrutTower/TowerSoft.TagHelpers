using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Disables a link if the enabled attribute is false. Requires a .disable-link CSS class with 'pointer-events: none' style.
    /// </summary>
    [HtmlTargetElement("a", Attributes = "asp-enabled")]
    public class AnchorEnabledTagHelper : TagHelper {
        /// <summary>
        /// Sets if the link is enabled or disabled
        /// </summary>
        [HtmlAttributeName("asp-enabled")]
        public bool Enabled { get; set; }


        /// <summary>
        /// Process method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            base.Process(context, output);

            if (!Enabled) {
                if (output.Attributes.ContainsName("href")) {
                    var href = output.Attributes["href"];
                    output.Attributes.Add("data-href", href.Value);
                    output.Attributes.Remove(href);
                }
                output.AddClass("disable-link", HtmlEncoder.Default);
            }
        }
    }
}
