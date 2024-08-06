using Microsoft.AspNetCore.Html;
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
    /// Horizontal form group tag helper for select lists
    /// </summary>
    [HtmlTargetElement("hrFormSelect", Attributes = "asp-for")]
    public class HorizontalFormSelectTagHelper : TagHelper {
        /// <summary>
        /// HorizontalFormSelect Constructor
        /// </summary>
        /// <param name="htmlGenerator"></param>
        /// <param name="htmlHelper"></param>
        public HorizontalFormSelectTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) {
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
        public string OptionLabel { get; set; }

        /// <summary>
        /// Bootstrap column CSS for the label. If not set, defaults to: col-md-4 col-lg-3
        /// </summary>
        [HtmlAttributeName("label-col")]
        public string LabelCol { get; set; }

        /// <summary>
        /// Bootstrap column CSS for the input. If not set, defaults to: col-md-7 col-lg-6
        /// </summary>
        [HtmlAttributeName("input-col")]
        public string InputCol { get; set; }

        /// <summary>
        /// Sets the CSS class on the input. Overrides the default Bootstrap class
        /// </summary>
        [HtmlAttributeName("input-css")]
        public string InputCss { get; set; }

        /// <summary>
        /// Override the label name
        /// </summary>
        [HtmlAttributeName("label")]
        public string LabelName { get; set; }

        /// <summary>
        /// Set the multiple attribute on the select element
        /// </summary>
        public bool Multiple { get; set; } = false;

        /// <summary></summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "div";
            output.AddClass("row", HtmlEncoder.Default);
            output.AddClass("mb-3", HtmlEncoder.Default);

            string labelColumnCss = LabelCol ?? "col-md-4 col-lg-3";
            string fieldColumnCss = InputCol ?? "col-md-7 col-lg-6";

            TagBuilder labelDiv = new("div");
            labelDiv.AddCssClass(labelColumnCss + " text-md-end");
            TagBuilder fieldDiv = new("div");
            fieldDiv.AddCssClass(fieldColumnCss);

            Dictionary<string, string> htmlAttributes = new();
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

            TagHelperUtilities utils = new(For, HtmlGenerator, HtmlHelper, ViewContext);

            TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, LabelName);
            IHtmlContent inputElement = await utils.CreateSelectElement(context, Items, Multiple, OptionLabel, htmlAttributes);
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
