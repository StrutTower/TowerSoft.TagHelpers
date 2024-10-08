﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Form group tag helper for select lists
    /// </summary>
    [HtmlTargetElement("formSelect", Attributes = "asp-for")]
    public class FormSelectTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) : TagHelper {

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
        public string OptionLabel { get; set; }

        /// <summary>
        /// Sets CSS on the input. Overrides the default Bootstrap class
        /// </summary>
        [HtmlAttributeName("input-css")]
        public string InputCss { get; set; }

        /// <summary>
        /// Overrides the label display text
        /// </summary>
        [HtmlAttributeName("label")]
        public string LabelName { get; set; }

        public bool Multiple { get; set; } = false;

        /// <summary></summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary></summary>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("mb-3", HtmlEncoder.Default);

            Dictionary<string, string> htmlAttributes = [];
            if (context.AllAttributes.ContainsName("autofocus")) {
                htmlAttributes.Add("autofocus", string.Empty);
            }
            if (!string.IsNullOrWhiteSpace(InputCss)) {
                htmlAttributes.Add("class", InputCss);
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

            TagHelperUtilities utils = new(For, htmlGenerator, htmlHelper, ViewContext);

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
