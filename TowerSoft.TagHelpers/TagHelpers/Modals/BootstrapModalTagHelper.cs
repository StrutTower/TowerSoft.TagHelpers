using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Options;

namespace TowerSoft.TagHelpers.TagHelpers.Modals {
    /// <summary>
    /// Renders a Bootstrap modal element. Use bs-modal-body and bs-modal-footer to render the content.
    /// </summary>
    [HtmlTargetElement("bs-modal"), RestrictChildren("bs-modal-body", "bs-modal-footer")]
    public class BootstrapModalTagHelper : TagHelper {

        /// <summary>
        /// Title of the modal.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ID value of the modal.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Toggles if the modal-header is rendered.
        /// </summary>
        /// <remarks>Default is true.</remarks>
        public bool ShowHeader { get; set; } = true;

        /// <summary>
        /// Set if the .modal-dialog element should internally scroll in the modal.
        /// <br />Documentation: <see href="https://getbootstrap.com/docs/5.3/components/modal/#scrolling-long-content">https://getbootstrap.com/docs/5.3/components/modal/#scrolling-long-content</see>
        /// </summary>
        /// <remarks>Default is false.</remarks>
        public bool DialogScrollable { get; set; }

        /// <summary>
        /// Set if the modal should be centered in the viewport.
        /// <br />Documentation: <see href="https://getbootstrap.com/docs/5.3/components/modal/#vertically-centered">https://getbootstrap.com/docs/5.3/components/modal/#vertically-centered</see>
        /// </summary>
        /// <remarks>Default is false.</remarks>
        public bool VerticallyCentered { get; set; }

        /// <summary>
        /// Set the modal backdrop to static which prevents closing the modal by clicking on the backdrop.
        /// Also sets keyboard to false which prevent closing the modal with the ESC key.
        /// <br />Documentation: <see href="https://getbootstrap.com/docs/5.3/components/modal/#static-backdrop">https://getbootstrap.com/docs/5.3/components/modal/#static-backdrop</see>
        /// </summary>
        /// <remarks>Default value is false.</remarks>
        public bool StaticBackdrop { get; set; }

        /// <summary>
        /// Optional. Bootstrap modal width value.
        /// <br />Documentation: <see href="https://getbootstrap.com/docs/5.3/components/modal/#optional-sizes">https://getbootstrap.com/docs/5.3/components/modal/#optional-sizes</see>
        /// </summary>
        /// <remarks>Default width is equivalent to modal-md.</remarks>
        public ModalWidth? ModalWidth { get; set; }

        /// <summary>
        /// Optional. Sets the Bootstrap modal fullscreen classes.
        /// <br />Documentation: <see href="https://getbootstrap.com/docs/5.3/components/modal/#fullscreen-modal">https://getbootstrap.com/docs/5.3/components/modal/#fullscreen-modal</see>
        /// </summary>
        public ModalFullscreen? ModalFullscreen { get; set; }

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            BootstrapModalContext modalContext = new BootstrapModalContext();
            context.Items.Add(typeof(BootstrapModalTagHelper), modalContext);

            await output.GetChildContentAsync();

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            string classNames = "modal fade";
            if (output.Attributes.ContainsName("class")) {
                classNames = string.Format("{0} {1}", output.Attributes["class"].Value, classNames);
            }
            output.Attributes.SetAttribute("class", classNames);
            output.Attributes.Add("id", ID);
            output.Attributes.Add("role", "dialog");
            output.Attributes.Add("tabindex", "-1");

            if (StaticBackdrop) {
                output.Attributes.Add("data-bs-backdrop", "static");
                output.Attributes.Add("data-bs-keyboard", "false");
            }

            // Create TagBuilders
            TagBuilder dialog = new("div");
            dialog.AddCssClass("modal-dialog");
            if (ModalWidth != null)
                dialog.AddCssClass(ModalWidth.ToString().Replace("_", "-"));
            if (ModalFullscreen != null)
                dialog.AddCssClass(ModalFullscreen.ToString().Replace("_", "-"));
            if (DialogScrollable)
                dialog.AddCssClass("modal-dialog-scrollable");
            if (VerticallyCentered)
                dialog.AddCssClass("modal-dialog-centered");

            TagBuilder content = new("div");
            content.AddCssClass("modal-content");

            TagBuilder header = new("div");
            header.AddCssClass("modal-header");

            TagBuilder title = new("h1");
            title.AddCssClass("modal-title");
            title.AddCssClass("fs-5");
            title.InnerHtml.Append(Title);

            TagBuilder closeButton = new("button");
            closeButton.AddCssClass("btn-close");
            closeButton.Attributes.Add("type", "button");
            closeButton.Attributes.Add("data-bs-dismiss", "modal");
            closeButton.Attributes.Add("aria-label", "Close");

            TagBuilder body = new("div");
            if (!string.IsNullOrWhiteSpace(modalContext.BodyClass))
                body.Attributes.Add("class", modalContext.BodyClass);
            body.AddCssClass("modal-body");
            if (modalContext.Body != null)
                body.InnerHtml.AppendHtml(modalContext.Body);

            // Append all TagBuilders
            header.InnerHtml.AppendHtml(title);
            header.InnerHtml.AppendHtml(closeButton);

            if (ShowHeader)
                content.InnerHtml.AppendHtml(header);
            content.InnerHtml.AppendHtml(body);

            if (modalContext.Footer != null) {
                TagBuilder footer = new("div");
                if (!string.IsNullOrWhiteSpace(modalContext.FooterClass))
                    footer.Attributes.Add("class", modalContext.FooterClass);
                footer.AddCssClass("modal-footer");
                footer.InnerHtml.AppendHtml(modalContext.Footer);
                content.InnerHtml.AppendHtml(footer);
            }

            dialog.InnerHtml.AppendHtml(content);
            output.Content.AppendHtml(dialog);
        }
    }
}
