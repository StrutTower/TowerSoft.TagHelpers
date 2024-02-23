using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Similar to the hrFormField this generates a label, description and ASP.NET validation message but the input element is taken from the inner HTML of the tag helper.
    /// </summary>
    [HtmlTargetElement("hrFormContainer", Attributes = "asp-for")]
    public class HorizontalFormContainerTagHelper : TagHelper {

        public HorizontalFormContainerTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) {
            HtmlGenerator = htmlGenerator;
            HtmlHelper = htmlHelper;
        }

        public IHtmlGenerator HtmlGenerator { get; }
        public IHtmlHelper HtmlHelper { get; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

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

        /// <summary></summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "div";
            output.AddClass("row", HtmlEncoder.Default);
            output.AddClass("mb-3", HtmlEncoder.Default);

            TagHelperUtilities utils = new(For, HtmlGenerator, HtmlHelper, ViewContext);

            string labelColumnCss = LabelCol ?? "col-md-4 col-lg-3";
            string fieldColumnCss = InputCol ?? "col-md-7 col-lg-6";

            TagBuilder labelDiv = new("div");
            labelDiv.AddCssClass(labelColumnCss + " text-md-end");
            TagBuilder fieldDiv = new("div");
            fieldDiv.AddCssClass(fieldColumnCss);

            TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label);
            TagHelperOutput validationMessageElement = await utils.CreateValidationMessageElement(context);
            TagHelperOutput descriptionElement = await utils.CreateDescriptionElement(context);

            labelDiv.InnerHtml.AppendHtml(labelElement);
            fieldDiv.InnerHtml.AppendHtml(await output.GetChildContentAsync());
            fieldDiv.InnerHtml.AppendHtml(validationMessageElement);
            fieldDiv.InnerHtml.AppendHtml(descriptionElement);

            output.Content.AppendHtml(labelDiv);
            output.Content.AppendHtml(fieldDiv);
        }
    }
}
