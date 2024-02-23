using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Enums;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Renders a horizontal Bootstrap style form group with a label, input, description, and ASP.NET validation message
    /// </summary>
    [HtmlTargetElement("hrFormField", Attributes = "asp-for")]
    public class HorizontalFormFieldTagHelper : TagHelper {
        private AutocompleteSetting _autocompleteSetting;

        public HorizontalFormFieldTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) {
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
        /// Bootstrap column CSS for the label. If not set, defaults to: col-md-4 col-lg-3
        /// </summary>
        public string? LabelCol { get; set; }

        /// <summary>
        /// Bootstrap column CSS for the input. If not set, defaults to: col-md-7 col-lg-6
        /// </summary>
        public string? InputCol { get; set; }

        /// <summary>
        /// Sets CSS on the input. Overrides the default Bootstrap class
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
            output.AddClass("row", HtmlEncoder.Default);
            output.AddClass("mb-3", HtmlEncoder.Default);

            TagHelperUtilities utils = new(For, HtmlGenerator, HtmlHelper, ViewContext);

            Type type = For.Metadata.ModelType;
            type = Nullable.GetUnderlyingType(type) ?? type;

            string labelColumnCss = LabelCol ?? "col-md-4 col-lg-3";
            string fieldColumnCss = InputCol ?? "col-md-7 col-lg-6";

            if (type == typeof(bool) && string.IsNullOrWhiteSpace(Renderer)) {
                TagBuilder labelDiv = new("div");
                labelDiv.AddCssClass(labelColumnCss);
                TagBuilder fieldDiv = new("div");
                fieldDiv.AddCssClass(fieldColumnCss);

                // Handle Booleans/Checkbox
                TagBuilder formCheck = new("div");
                formCheck.AddCssClass("form-check");
                formCheck.InnerHtml.AppendHtml(utils.CreateInputElement(null, InputCss));
                formCheck.InnerHtml.AppendHtml(await utils.CreateLabelElement(context, Label, "form-check-label"));
                fieldDiv.InnerHtml.AppendHtml(formCheck);
                fieldDiv.InnerHtml.AppendHtml(await utils.CreateValidationMessageElement(context));
                fieldDiv.InnerHtml.AppendHtml(await utils.CreateDescriptionElement(context));

                output.Content.AppendHtml(labelDiv);
                output.Content.AppendHtml(fieldDiv);
            } else {
                TagBuilder labelDiv = new("div");
                labelDiv.AddCssClass(labelColumnCss + " text-md-end");
                TagBuilder fieldDiv = new("div");
                fieldDiv.AddCssClass(fieldColumnCss);

                Dictionary<string, string> htmlAttributes = new() {
                    { "autocomplete", Autocomplete.ToString() }
                };
                if (!string.IsNullOrWhiteSpace(Placeholder)) {
                    htmlAttributes.Add("placeholder", Placeholder);
                }
                if (context.AllAttributes.ContainsName("autofocus")) {
                    htmlAttributes.Add("autofocus", string.Empty);
                }
                foreach(var attr in context.AllAttributes.Where(x => x.Name.StartsWith("data-"))) {
                    htmlAttributes.Add(attr.Name, attr.Value.ToString());
                }

                // Default editor or supplied editor template
                TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label);
                IHtmlContent inputElement = utils.CreateInputElement(Renderer, InputCss, htmlAttributes);
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
