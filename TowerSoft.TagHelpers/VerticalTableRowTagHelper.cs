using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TowerSoft.TagHelpers.Extensions;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Generates a table row to display the value of a property with a th and td.
    /// The th will have the display name of the property. The td will have the value of the property
    /// </summary>
    /// <remarks>VerticalTableRow Constructor</remarks>
    [HtmlTargetElement("tr-display")]
    public class VerticalTableRowTagHelper(IHtmlHelper htmlHelper) : TagHelper {

        /// <summary>
        /// Model expression for the property
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression Model { get; set; }

        /// <summary>
        /// Override the label name used in the th element
        /// </summary>
        [HtmlAttributeName("label")]
        public string LabelName { get; set; }

        /// <summary>
        /// Name of the display template to use to display the value
        /// </summary>
        [HtmlAttributeName("template")]
        public string TemplateName { get; set; }

        /// <summary>
        /// Optional text to show if the value is null or an empty string
        /// </summary>
        [HtmlAttributeName("null-text")]
        public string NullText { get; set; }

        /// <summary>
        /// Hides the entire row if the model expression is null. Default false
        /// </summary>
        [HtmlAttributeName("hide-null")]
        public bool HideNull { get; set; }

        /// <summary></summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (Model != null) {
                if (HideNull && (Model.Model == null || (Model.Model is string modelStr && string.IsNullOrWhiteSpace(modelStr)))) {
                    output.SuppressOutput();
                } else {
                    ((IViewContextAware)htmlHelper).Contextualize(ViewContext);
                    output.TagName = "tr";

                    TagBuilder th = new("th");
                    if (string.IsNullOrWhiteSpace(LabelName))
                        th.InnerHtml.SetContent(Model.Metadata.GetDisplayName());
                    else
                        th.InnerHtml.SetContent(LabelName);

                    TagBuilder td = new("td");
                    if (!string.IsNullOrWhiteSpace(NullText) && (Model.Model == null || (Model.Model is string modelStr2 && string.IsNullOrWhiteSpace(modelStr2)))) {
                        td.AddCssClass("twr-taghelper-tr-display-null");
                        td.InnerHtml.SetHtmlContent(NullText);
                    } else {
                        td.InnerHtml.SetHtmlContent(TagHelperUtilities.TagHelperDisplay(htmlHelper, Model, TemplateName).ToRawString());
                    }

                    output.Content.SetHtmlContent(th.ToRawString() + td.ToRawString());
                }
            }
        }
    }
}
