﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Enums;
using TowerSoft.TagHelpers.Options;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Renders a horizontal Bootstrap style form group with a label, input, description, and ASP.NET validation message
    /// </summary>
    [HtmlTargetElement("hrFormField", Attributes = "asp-for")]
    public class HorizontalFormFieldTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) : TagHelper {
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
        /// Bootstrap column CSS for the label. If not set, defaults to: col-md-4 col-lg-3
        /// </summary>
        public string LabelCol { get; set; }

        /// <summary>
        /// Bootstrap column CSS for the input. If not set, defaults to: col-md-7 col-lg-6
        /// </summary>
        public string InputCol { get; set; }

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
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            ((IViewContextAware)htmlHelper).Contextualize(ViewContext);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("row", HtmlEncoder.Default);
            output.AddClass("mb-3", HtmlEncoder.Default);

            TagHelperUtilities utils = new(For, htmlGenerator, htmlHelper, ViewContext);

            Type type = For.Metadata.ModelType;
            type = Nullable.GetUnderlyingType(type) ?? type;

            string labelColumnCss = LabelCol ?? "col-md-4 col-lg-3";
            string fieldColumnCss = InputCol ?? "col-md-7 col-lg-6";

            if (inputAttributes == null)
                inputAttributes = [];

            if (type == typeof(bool) && (string.IsNullOrWhiteSpace(Renderer) || Renderer == HtmlRenderer.Boolean)) {
                // Handle Booleans/Checkbox
                PropertyInfo prop = For.Metadata.ContainerType.GetProperty(For.Metadata.Name);
                bool required = false;
                bool nullable = For.ModelExplorer.Metadata.IsNullableValueType;

                if (prop != null)
                    required = prop.IsDefined(typeof(RequiredAttribute), true);


                TagBuilder labelDiv = new("div");
                labelDiv.AddCssClass(labelColumnCss + " text-md-end");
                TagBuilder fieldDiv = new("div");
                fieldDiv.AddCssClass(fieldColumnCss);

                if (required || !nullable) {
                    TagBuilder formCheck = new("div");
                    formCheck.AddCssClass("form-check");
                    formCheck.InnerHtml.AppendHtml(utils.CreateInputElement(null, InputCss, inputAttributes));
                    formCheck.InnerHtml.AppendHtml(await utils.CreateLabelElement(context, Label, "form-check-label"));
                    fieldDiv.InnerHtml.AppendHtml(formCheck);
                } else {
                    TagHelperAttribute labelsAttribute = context.AllAttributes.SingleOrDefault(x => x.Name == "data-labels");
                    if (labelsAttribute != null)
                        inputAttributes.Add(labelsAttribute.Name, labelsAttribute.Value.ToString());
                    TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label);
                    labelDiv.InnerHtml.AppendHtml(labelElement);
                    fieldDiv.InnerHtml.AppendHtml(utils.CreateInputElement(HtmlRenderer.Boolean, InputCss, inputAttributes));
                }

                output.Content.AppendHtml(labelDiv);
                output.Content.AppendHtml(fieldDiv);
            } else {
                TagBuilder labelDiv = new("div");
                labelDiv.AddCssClass(labelColumnCss + " text-md-end");
                TagBuilder fieldDiv = new("div");
                fieldDiv.AddCssClass(fieldColumnCss);

                if (!inputAttributes.ContainsKey("autocomplete"))
                    inputAttributes.Add("autocomplete", Autocomplete.ToString());
                if (!string.IsNullOrWhiteSpace(Placeholder) && !inputAttributes.ContainsKey("placeholder")) {
                    inputAttributes.Add("placeholder", Placeholder);
                }
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

                // Default editor or supplied editor template
                TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label);
                IHtmlContent inputElement = utils.CreateInputElement(Renderer, InputCss, inputAttributes);
                TagHelperOutput validationMessageElement = await utils.CreateValidationMessageElement(context);
                TagHelperOutput descriptionElement = await utils.CreateDescriptionElement(context);

                labelDiv.InnerHtml.AppendHtml(labelElement);
                fieldDiv.InnerHtml.AppendHtml(inputElement);
                fieldDiv.InnerHtml.AppendHtml(validationMessageElement);
                fieldDiv.InnerHtml.AppendHtml(descriptionElement);

                output.Content.AppendHtml(labelDiv);
                output.Content.AppendHtml(fieldDiv);
            }
        }
    }
}
