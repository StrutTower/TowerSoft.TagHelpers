using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace TowerSoft.TagHelpers.TagHelpers.Modals {
    /// <summary>
    /// Renders the Bootstrap modal body. If this is not supplied the body will be empty.
    /// </summary>
    [HtmlTargetElement("bs-modal-body", ParentTag = "bs-modal")]
    public class BootstrapModalBodyTagHelper : TagHelper {

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            TagHelperContent childContent = await output.GetChildContentAsync();
            BootstrapModalContext modalContext = (BootstrapModalContext)context.Items[typeof(BootstrapModalTagHelper)];
            modalContext.Body = childContent;
            if (output.Attributes.ContainsName("class"))
                modalContext.BodyClass = output.Attributes["class"].Value.ToString();
            output.SuppressOutput();
        }
    }
}
