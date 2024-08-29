using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Enums;
using TowerSoft.TagHelpers.Utilities;

// Source: https://stackoverflow.com/questions/47547844/tag-helper-embedded-in-another-tag-helpers-code-doesnt-render
// Source: https://stackoverflow.com/questions/48322431/what-is-the-tag-helper-equivalent-of-html-editorfor
namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Renders a Bootstrap style form group with a label, input, description, and ASP.NET validation message
    /// </summary>
    [HtmlTargetElement("formField", Attributes = "asp-for")]
    public class FormFieldTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) : TagHelper {
        private AutocompleteSetting _autocompleteSetting;

        /// <summary>Model</summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Sets the renderer used for this field. Default will be based on the datatype of the property
        /// </summary>
        public string Renderer { get; set; }

        /// <summary>
        /// Overrides the label display text
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Sets CSS on the input. Overrides the default Bootstrap class
        /// </summary>
        public string InputCss { get; set; }

        /// <summary>
        /// Sets the placeholder text for the input
        /// </summary>
        public string Placeholder { get; set; }

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

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            (htmlHelper as IViewContextAware).Contextualize(ViewContext);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "mb-3");

            TagHelperUtilities utils = new(For, htmlGenerator, htmlHelper, ViewContext);

            Type type = For.Metadata.ModelType;
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(bool) && string.IsNullOrWhiteSpace(Renderer)) {
                // Handle Booleans/Checkbox
                TagBuilder formCheck = new("div");
                formCheck.AddCssClass("form-check");
                formCheck.InnerHtml.AppendHtml(utils.CreateInputElement(null, InputCss));
                formCheck.InnerHtml.AppendHtml(await utils.CreateLabelElement(context, Label, "form-check-label"));
                output.Content.AppendHtml(formCheck);

                output.Content.AppendHtml(await utils.CreateValidationMessageElement(context));
                output.Content.AppendHtml(await utils.CreateDescriptionElement(context));
            } else {
                Dictionary<string, string> htmlAttributes = new() {
                    { "autocomplete", Autocomplete.ToString() }
                };
                if (!string.IsNullOrWhiteSpace(Placeholder)) {
                    htmlAttributes.Add("placeholder", Placeholder);
                }
                if (context.AllAttributes.ContainsName("autofocus")) {
                    htmlAttributes.Add("autofocus", string.Empty);
                }
                if (context.AllAttributes.ContainsName("disabled")) {
                    htmlAttributes.Add("disabled", string.Empty);
                }
                if (context.AllAttributes.ContainsName("readonly")) {
                    htmlAttributes.Add("readonly", string.Empty);
                }
                foreach (var attr in context.AllAttributes.Where(x => x.Name.StartsWith("data-"))) {
                    htmlAttributes.Add(attr.Name, attr.Value.ToString());
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
