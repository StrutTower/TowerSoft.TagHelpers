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
    [HtmlTargetElement("tr-display")]
    public class VerticalTableRowTagHelper : TagHelper {
        /// <summary>VerticalTableRow Constructor</summary>
        public VerticalTableRowTagHelper(IHtmlHelper htmlHelper) {
            HtmlHelper = htmlHelper;
        }

        private IHtmlHelper HtmlHelper { get; set; }

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
                ((IViewContextAware)HtmlHelper).Contextualize(ViewContext);
                output.TagName = "tr";

                TagBuilder th = new TagBuilder("th");
                if (string.IsNullOrWhiteSpace(LabelName))
                    th.InnerHtml.SetContent(Model.Metadata.GetDisplayName());
                else
                    th.InnerHtml.SetContent(LabelName);

                TagBuilder td = new TagBuilder("td");
                if (Model.Model is bool boolean) {
                    td.InnerHtml.SetHtmlContent(BooleanIconAndText(boolean));
                } else {
                    td.InnerHtml.SetHtmlContent(TagHelperUtilities.TagHelperDisplay(HtmlHelper, Model, TemplateName).ToRawString());
                }

                output.Content.SetHtmlContent(th.ToRawString() + td.ToRawString());
            }
        }

        private IHtmlContent BooleanIconAndText(bool? boolean) {
            TagBuilder tag = new TagBuilder("span");
            if (boolean.HasValue && boolean.Value) {
                tag.AddCssClass("mdi mdi-checkbox-marked-outline text-success");
                tag.Attributes.Add("title", "True");
            } else if (boolean.HasValue) {
                tag.AddCssClass("mdi mdi-checkbox-blank-off-outline text-danger");
                tag.Attributes.Add("title", "False");
            } else {
                tag.AddCssClass("mdi mdi-checkbox-blank-outline text-secondary");
                tag.Attributes.Add("title", "Unknown");
            }
            TagBuilder container = new TagBuilder("span");
            container.InnerHtml.AppendHtml(tag.ToRawString() + " ");
            if (boolean.HasValue && boolean.Value) {
                container.InnerHtml.AppendHtml("True");
            } else if (boolean.HasValue) {
                container.InnerHtml.AppendHtml("False");
            } else {
                container.InnerHtml.AppendHtml("Unknown");
            }
            return container;
        }
    }
}
