using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Enums;
using TowerSoft.TagHelpers.Options;
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
        private Dictionary<string, string> inputAttributes;

        private const string ModelExpressionName = "asp-for";
        private const string InputAttributeDictionaryName = "asp-all-input-attributes";
        private const string InputAttributePrefix = "asp-input-attribute-";

        /// <summary>Model</summary>
        [HtmlAttributeName(ModelExpressionName)]
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
        /// Dictonary to set custom attributes on the input element
        /// </summary>
        [HtmlAttributeName(InputAttributeDictionaryName, DictionaryAttributePrefix = InputAttributePrefix)]
        public Dictionary<string, string> InputAttributes {
            get {
                inputAttributes ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                return inputAttributes;
            }
            set {
                inputAttributes = value;
            }
        }

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

            if (type == typeof(bool) && (string.IsNullOrWhiteSpace(Renderer) || Renderer == HtmlRenderer.Boolean)) {
                // Handle Booleans/Checkbox
                PropertyInfo prop = For.Metadata.ContainerType.GetProperty(For.Metadata.Name);
                bool required = false;
                bool nullable = For.ModelExplorer.Metadata.IsNullableValueType;

                if (prop != null)
                    required = prop.IsDefined(typeof(RequiredAttribute), true);

                if (required || !nullable) {
                    TagBuilder formCheck = new("div");
                    formCheck.AddCssClass("form-check");
                    formCheck.InnerHtml.AppendHtml(utils.CreateInputElement(null, InputCss, inputAttributes));
                    formCheck.InnerHtml.AppendHtml(await utils.CreateLabelElement(context, Label, "form-check-label"));
                    output.Content.AppendHtml(formCheck);
                } else {
                    TagHelperAttribute labelsAttribute = context.AllAttributes.SingleOrDefault(x => x.Name == "data-labels");
                    if (labelsAttribute != null)
                        inputAttributes.Add(labelsAttribute.Name, labelsAttribute.Value.ToString());
                    output.Content.AppendHtml(await utils.CreateLabelRequiredElement(context, Label));
                    output.Content.AppendHtml(utils.CreateInputElement(HtmlRenderer.Boolean, InputCss, inputAttributes));
                }

                output.Content.AppendHtml(await utils.CreateValidationMessageElement(context));
                output.Content.AppendHtml(await utils.CreateDescriptionElement(context));
            } else {
                if (inputAttributes == null)
                    inputAttributes = [];

                if (!inputAttributes.ContainsKey("autocomplete"))
                    inputAttributes.Add("autocomplete", Autocomplete.ToString());

                if (!string.IsNullOrWhiteSpace(Placeholder) && !inputAttributes.ContainsKey("placeholder")) {
                    inputAttributes.Add("placeholder", Placeholder);
                }

                // Legacy Support before adding InputAttributes dictionary
                if (context.AllAttributes.ContainsName("autofocus") && !inputAttributes.ContainsKey("autofocus")) {
                    inputAttributes.Add("autofocus", string.Empty);
                }
                if (context.AllAttributes.ContainsName("disabled") && !inputAttributes.ContainsKey("disabled")) {
                    inputAttributes.Add("disabled", string.Empty);
                }
                if (context.AllAttributes.ContainsName("readonly") && !inputAttributes.ContainsKey("readonly")) {
                    inputAttributes.Add("readonly", string.Empty);
                }
                foreach (TagHelperAttribute attr in context.AllAttributes.Where(x => x.Name.StartsWith("data-"))) {
                    if (!inputAttributes.ContainsKey(attr.Name))
                        inputAttributes.Add(attr.Name, attr.Value.ToString());
                }
                //----

                // Default editor or supplied renderer
                TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label);
                IHtmlContent inputElement = utils.CreateInputElement(Renderer, InputCss, inputAttributes);
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
