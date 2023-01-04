using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Utilities;

// Source: https://stackoverflow.com/questions/47547844/tag-helper-embedded-in-another-tag-helpers-code-doesnt-render
// Source: https://stackoverflow.com/questions/48322431/what-is-the-tag-helper-equivalent-of-html-editorfor
namespace TowerSoft.TagHelpers {
    [HtmlTargetElement("formField", Attributes = "asp-for")]
    public class FormFieldTagHelper : TagHelper {
        public FormFieldTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) {
            HtmlGenerator = htmlGenerator;
            HtmlHelper = htmlHelper;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public string? Template { get; set; }

        public string? Label { get; set; }

        public string? InputCss { get; set; }

        public IHtmlGenerator HtmlGenerator { get; }

        public IHtmlHelper HtmlHelper { get; }

        /// <summary></summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            ((IViewContextAware)HtmlHelper).Contextualize(ViewContext);

            output.TagName = "div";
            output.Attributes.SetAttribute("class", "mb-3");

            TagHelperUtilities utils = new TagHelperUtilities(For, HtmlGenerator, HtmlHelper, ViewContext);

            Type type = For.Metadata.ModelType;
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(bool) && string.IsNullOrWhiteSpace(Template)) {
                // Handle Booleans/Checkbox
                TagBuilder formCheck = new TagBuilder("div");
                formCheck.AddCssClass("form-check");
                formCheck.InnerHtml.AppendHtml(utils.CreateInputElement(null, InputCss));
                formCheck.InnerHtml.AppendHtml(await utils.CreateLabelElement(context, Label, "form-check-label"));
                output.Content.AppendHtml(formCheck);

                output.Content.AppendHtml(await utils.CreateValidationMessageElement(context));
                output.Content.AppendHtml(await utils.CreateDescriptionElement(context));
            } else {
                // Default editor or supplied editor template
                TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label);
                IHtmlContent inputElement = utils.CreateInputElement(Template, InputCss);
                TagHelperOutput validationMessageElement = await utils.CreateValidationMessageElement(context);
                TagHelperOutput descriptionElement = await utils.CreateDescriptionElement(context);

                output.Content.AppendHtml(labelElement);
                output.Content.AppendHtml(inputElement);
                output.Content.AppendHtml(validationMessageElement);
                output.Content.AppendHtml(descriptionElement);
            }
        }
    }
}
