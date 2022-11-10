using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Displays the value of a property similar to Html.DisplayFor
    /// </summary>
    [HtmlTargetElement("div", Attributes = "display-for")]
    [HtmlTargetElement("span", Attributes = "display-for")]
    [HtmlTargetElement("a", Attributes = "display-for")]
    [HtmlTargetElement("abbr", Attributes = "display-for")]
    [HtmlTargetElement("p", Attributes = "display-for")]
    [HtmlTargetElement("em", Attributes = "display-for")]
    [HtmlTargetElement("strong", Attributes = "display-for")]
    [HtmlTargetElement("th", Attributes = "display-for")]
    [HtmlTargetElement("td", Attributes = "display-for")]
    [HtmlTargetElement("h1", Attributes = "display-for")]
    [HtmlTargetElement("h2", Attributes = "display-for")]
    [HtmlTargetElement("h3", Attributes = "display-for")]
    [HtmlTargetElement("h4", Attributes = "display-for")]
    [HtmlTargetElement("h5", Attributes = "display-for")]
    [HtmlTargetElement("h6", Attributes = "display-for")]
    [HtmlTargetElement("li", Attributes = "display-for")]
    [HtmlTargetElement("option", Attributes = "display-for")]
    [HtmlTargetElement("breadcrumb-item", Attributes = "display-for")]
    public class DisplayForTagHelper : TagHelper {
        /// <summary>
        /// DisplayForTagHelper constructor
        /// </summary>
        /// <param name="htmlHelper"></param>
        public DisplayForTagHelper(IHtmlHelper htmlHelper) {
            HtmlHelper = htmlHelper;
        }

        private IHtmlHelper HtmlHelper { get; set; }

        /// <summary>
        /// Model expression for the property
        /// </summary>
        [HtmlAttributeName("display-for")]
        public ModelExpression Model { get; set; }

        /// <summary>
        /// Name of the display template
        /// </summary>
        [HtmlAttributeName("template")]
        public string TemplateName { get; set; }

        /// <summary>
        /// If set to true the display name will be added after any content already in the element
        /// </summary>
        public bool Append { get; set; } = false;

        /// <summary></summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            ((IViewContextAware)HtmlHelper).Contextualize(ViewContext);
            if (Append)
                output.PostContent.AppendHtml(TagHelperUtilities.TagHelperDisplay(HtmlHelper, Model, TemplateName));
            else
                output.PreContent.AppendHtml(TagHelperUtilities.TagHelperDisplay(HtmlHelper, Model, TemplateName));
        }
    }
}
