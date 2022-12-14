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
        public string LabelCol { get; set; }

        /// <summary>
        /// Bootstrap column CSS for the input. If not set, defaults to: col-md-7 col-lg-6
        /// </summary>
        public string InputCol { get; set; }

        /// <summary>
        /// Override the label name
        /// </summary>
        public string LabelName { get; set; }

        /// <summary>
        /// Set the multiple attribute on the select element
        /// </summary>
        public bool Multiple { get; set; } = false;

        /// <summary></summary>
        public IHtmlGenerator HtmlGenerator { get; }

        /// <summary></summary>
        public IHtmlHelper HtmlHelper { get; }

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

            TagBuilder labelDiv = new TagBuilder("div");
            labelDiv.AddCssClass(labelColumnCss + " text-md-end");
            TagBuilder fieldDiv = new TagBuilder("div");
            fieldDiv.AddCssClass(fieldColumnCss);

            TagHelperUtilities utils = new TagHelperUtilities(For, HtmlGenerator, HtmlHelper, ViewContext);

            TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, LabelName);
            IHtmlContent inputElement = await utils.CreateSelectElement(context, Items, Multiple, OptionLabel);
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
