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
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Form group tag helper for select lists
    /// </summary>
    [HtmlTargetElement("formSelect", Attributes = "asp-for")]
    public class FormSelectTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) : TagHelper {
        private Dictionary<string, string> selectAttributes;

        private const string ModelExpressionName = "asp-for";
        private const string ListItemsName = "asp-items";
        private const string OptionLabelName = "asp-option-label";
        private const string SelectAttributeDictionaryName = "asp-all-select-attributes";
        private const string SelectAttributePrefix = "asp-select-attribute-";

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ModelExpressionName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// A collection of <see cref="SelectListItem"/> objects used to populate the &lt;select&gt; element with
        /// &lt;optgroup&gt; and &lt;option&gt; elements.
        /// </summary>
        [HtmlAttributeName(ListItemsName)]
        public IEnumerable<SelectListItem> Items { get; set; }

        /// <summary>
        /// The null or blank option. Set to null to exclude that option from the select list.
        /// </summary>
        [HtmlAttributeName(OptionLabelName)]
        public string OptionLabel { get; set; }

        /// <summary>
        /// Sets CSS on the input. Overrides the default Bootstrap class
        /// </summary>
        public string InputCss { get; set; }

        /// <summary>
        /// Overrides the label display text
        /// </summary>
        [HtmlAttributeName("label")]
        public string LabelName { get; set; }

        /// <summary>
        /// Sets select list to multiple if included. Does not need a value.
        /// </summary>
        [HtmlAttributeName("multiple")]
        public bool Multiple { get; set; } = false;

        /// <summary>
        /// Dictonary to set custom attributes on the select element
        /// </summary>
        [HtmlAttributeName(SelectAttributeDictionaryName, DictionaryAttributePrefix = SelectAttributePrefix)]
        public Dictionary<string, string> InputAttributes {
            get {
                selectAttributes ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                return selectAttributes;
            }
            set {
                selectAttributes = value;
            }
        }

        /// <summary></summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary></summary>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("mb-3", HtmlEncoder.Default);

            selectAttributes ??= [];

            // Legacy Support before adding InputAttributes dictionary
            if (context.AllAttributes.ContainsName("autofocus") && !selectAttributes.ContainsKey("autofocus")) {
                selectAttributes.Add("autofocus", string.Empty);
            }
            if (!string.IsNullOrWhiteSpace(InputCss) && !selectAttributes.ContainsKey("class")) {
                selectAttributes.Add("class", InputCss);
            }
            if (context.AllAttributes.ContainsName("disabled") && !selectAttributes.ContainsKey("disabled")) {
                selectAttributes.Add("disabled", string.Empty);
            }
            if (context.AllAttributes.ContainsName("readonly") && !selectAttributes.ContainsKey("readonly")) {
                selectAttributes.Add("readonly", string.Empty);
            }
            foreach (TagHelperAttribute attr in context.AllAttributes.Where(x => x.Name.StartsWith("data-"))) {
                if (!selectAttributes.ContainsKey(attr.Name))
                    selectAttributes.Add(attr.Name, attr.Value.ToString());
            }
            //----

            TagHelperUtilities utils = new(For, htmlGenerator, htmlHelper, ViewContext);

            TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, LabelName);
            IHtmlContent inputElement = await utils.CreateSelectElement(context, Items, Multiple, OptionLabel, selectAttributes);
            TagHelperOutput validationMessageElement = await utils.CreateValidationMessageElement(context);
            TagHelperOutput descriptionElement = await utils.CreateDescriptionElement(context);

            output.Content.AppendHtml(labelElement);
            output.Content.AppendHtml(inputElement);
            output.Content.AppendHtml(validationMessageElement);
            output.Content.AppendHtml(descriptionElement);
        }
    }
}
