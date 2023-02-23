using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Enums;
using TowerSoft.TagHelpers.Utilities;

// Source: https://stackoverflow.com/questions/47547844/tag-helper-embedded-in-another-tag-helpers-code-doesnt-render
// Source: https://stackoverflow.com/questions/48322431/what-is-the-tag-helper-equivalent-of-html-editorfor
namespace TowerSoft.TagHelpers {
    [HtmlTargetElement("formField", Attributes = "asp-for")]
    public class FormFieldTagHelper : TagHelper {
        private AutocompleteSetting _autocompleteSetting;

        public FormFieldTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) {
            HtmlGenerator = htmlGenerator;
            HtmlHelper = htmlHelper;
        }

        public IHtmlGenerator HtmlGenerator { get; }
        public IHtmlHelper HtmlHelper { get; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Sets the renderer used for this field. Default will be based on the datatype of the property
        /// </summary>
        public string? Renderer { get; set; }

        /// <summary>
        /// Overrides the label display text
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Set additional CSS on the input
        /// </summary>
        public string? InputCss { get; set; }

        /// <summary>
        /// Sets the placeholder text for the input
        /// </summary>
        public string? Placeholder { get; set; }

        /// <summary>
        /// Sets the autocomplete attribute on the input. Default is off
        /// </summary>
        public AutocompleteSetting Autocomplete {
            get => _autocompleteSetting;
            set {
                switch (value) {
                    case AutocompleteSetting.off:
                    case AutocompleteSetting.on:
                        _autocompleteSetting = value;
                        break;
                    default:
                        throw new ArgumentException(
                            message: "Invalid Autocomplete Value", paramName: nameof(value));
                }
            }
        }

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

            if (type == typeof(bool) && string.IsNullOrWhiteSpace(Renderer)) {
                // Handle Booleans/Checkbox
                TagBuilder formCheck = new TagBuilder("div");
                formCheck.AddCssClass("form-check");
                formCheck.InnerHtml.AppendHtml(utils.CreateInputElement(null, InputCss));
                formCheck.InnerHtml.AppendHtml(await utils.CreateLabelElement(context, Label, "form-check-label"));
                output.Content.AppendHtml(formCheck);

                output.Content.AppendHtml(await utils.CreateValidationMessageElement(context));
                output.Content.AppendHtml(await utils.CreateDescriptionElement(context));
            } else {
                Dictionary<string, string> htmlAttributes = new Dictionary<string, string> {
                    { "autocomplete", Autocomplete.ToString() }
                };
                if (!string.IsNullOrWhiteSpace(Placeholder)) {
                    htmlAttributes.Add("placeholder", Placeholder);
                }
                if (context.AllAttributes.ContainsName("autofocus")) {
                    htmlAttributes.Add("autofocus", string.Empty);
                }

                // Default editor or supplied editor template
                TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label);
                IHtmlContent inputElement = utils.CreateInputElement(Renderer, InputCss, htmlAttributes);
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
