using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// TagHelper to hide elements
    /// </summary>
    [HtmlTargetElement("*", Attributes = "asp-visible")]
    public class VisibleTagHelper : TagHelper {
        /// <summary>
        /// Sets if the element should be visible
        /// </summary>
        [HtmlAttributeName("asp-visible")]
        public bool Visible { get; set; }

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            base.Process(context, output);

            if (!Visible) output.Attributes.Add("hidden", "");
        }
    }
}
