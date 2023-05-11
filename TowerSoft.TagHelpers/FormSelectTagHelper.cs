using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Form group tag helper for select lists
    /// </summary>
    [HtmlTargetElement("formSelect", Attributes = "asp-for")]
    public class FormSelectTagHelper : TagHelper {
        public FormSelectTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) {
            HtmlGenerator = htmlGenerator;
            HtmlHelper = htmlHelper;
        }

        public IHtmlGenerator HtmlGenerator { get; }
        public IHtmlHelper HtmlHelper { get; }

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// A collection of <see cref="SelectListItem"/> objects used to populate the &lt;select&gt; element with
        /// &lt;optgroup&gt; and &lt;option&gt; elements.
        /// </summary>
        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> Items { get; set; }

        /// <summary>
        /// The null or blank option. Set to null to exclude that option from the select list.
        /// </summary>
        [HtmlAttributeName("asp-option-label")]
        public string? OptionLabel { get; set; }

        /// <summary>
        /// Set additional CSS on the input
        /// </summary>
        [HtmlAttributeName("input-css")]
        public string InputCss { get; set; }

        /// <summary>
        /// Overrides the label display text
        /// </summary>
        [HtmlAttributeName("label")]
        public string? LabelName { get; set; }

        public bool Multiple { get; set; } = false;

        /// <summary></summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "div";
            output.AddClass("mb-3", HtmlEncoder.Default);

            Dictionary<string, string> htmlAttributes = new Dictionary<string, string>();
            if (context.AllAttributes.ContainsName("autofocus")) {
                htmlAttributes.Add("autofocus", string.Empty);
            }
            if (!string.IsNullOrWhiteSpace(InputCss)) {
                htmlAttributes.Add("class", InputCss);
            }

            TagHelperUtilities utils = new TagHelperUtilities(For, HtmlGenerator, HtmlHelper, ViewContext);

            TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, LabelName);
            IHtmlContent inputElement = await utils.CreateSelectElement(context, Items, Multiple, OptionLabel, htmlAttributes);
            TagHelperOutput validationMessageElement = await utils.CreateValidationMessageElement(context);
            TagHelperOutput descriptionElement = await utils.CreateDescriptionElement(context);

            output.Content.AppendHtml(labelElement);
            output.Content.AppendHtml(inputElement);
            output.Content.AppendHtml(validationMessageElement);
            output.Content.AppendHtml(descriptionElement);
        }
    }
}
