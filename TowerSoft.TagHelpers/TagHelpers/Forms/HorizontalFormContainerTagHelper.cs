using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Options;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers.TagHelpers.Forms {
    /// <summary>
    /// Similar to the hrFormField this generates a label, description and ASP.NET validation message but the input element is taken from the inner HTML of the tag helper.
    /// </summary>
    [HtmlTargetElement("hrFormContainer", Attributes = "asp-for")]
    public class HorizontalFormContainerTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) : TagHelper {

        /// <summary>Model</summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

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
        /// Allows manually setting the required value for the red astrix
        /// </summary>
        public bool? ForceRequiredAstrix { get; set; }

        /// <summary></summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary></summary>
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            output.TagName = TowerSoftTagHelperSettings.HrFormFieldContainerElement ?? "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            if (TowerSoftTagHelperSettings.HrFormFieldContainerClass != null)
                output.Attributes.Add("class", TowerSoftTagHelperSettings.HrFormFieldContainerClass);

            TagHelperUtilities utils = new(For, htmlGenerator, htmlHelper, ViewContext);

            string labelColumnCss = null;
            string fieldColumnCss = null;

            if (LabelCol != null || TowerSoftTagHelperSettings.HrFormFieldLabelColumnClass != null)
                labelColumnCss = LabelCol ?? TowerSoftTagHelperSettings.HrFormFieldLabelColumnClass;

            if (InputCol != null || TowerSoftTagHelperSettings.HrFormFieldInputColumnClass != null)
                fieldColumnCss = InputCol ?? TowerSoftTagHelperSettings.HrFormFieldInputColumnClass;

            TagBuilder labelDiv = new("div");
            if (!string.IsNullOrWhiteSpace(labelColumnCss))
                labelDiv.AddCssClass(labelColumnCss);

            TagBuilder fieldDiv = new("div");
            if (!string.IsNullOrWhiteSpace(fieldColumnCss))
                fieldDiv.AddCssClass(fieldColumnCss);

            TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label, forceRequiredAstrix: ForceRequiredAstrix);
            TagHelperOutput validationMessageElement = await utils.CreateValidationMessageElement(context);
            TagHelperOutput descriptionElement = await utils.CreateDescriptionElement(context);

            labelDiv.InnerHtml.AppendHtml(labelElement);
            fieldDiv.InnerHtml.AppendHtml(await output.GetChildContentAsync());
            fieldDiv.InnerHtml.AppendHtml(descriptionElement);
            fieldDiv.InnerHtml.AppendHtml(validationMessageElement);

            output.Content.AppendHtml(labelDiv);
            output.Content.AppendHtml(fieldDiv);
        }
    }
}
