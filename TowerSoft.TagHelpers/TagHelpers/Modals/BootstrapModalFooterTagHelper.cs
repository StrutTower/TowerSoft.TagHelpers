using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace TowerSoft.TagHelpers.TagHelpers.Modals {
    /// <summary>
    /// Renders the Bootstrap modal footer. If this is not supplied the footer will be excluded from the modal.
    /// </summary>
    [HtmlTargetElement("bs-modal-footer", ParentTag = "bs-modal")]
    public class BootstrapModalFooterTagHelper : TagHelper {

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            TagHelperContent childContent = await output.GetChildContentAsync();
            BootstrapModalContext modalContext = (BootstrapModalContext)context.Items[typeof(BootstrapModalTagHelper)];
            modalContext.Footer = childContent;
            if (output.Attributes.ContainsName("class"))
                modalContext.FooterClass = output.Attributes["class"].Value.ToString();
            output.SuppressOutput();
        }
    }
}
