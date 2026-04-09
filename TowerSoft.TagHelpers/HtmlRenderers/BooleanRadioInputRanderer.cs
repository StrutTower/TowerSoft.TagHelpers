using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using TowerSoft.TagHelpers.Interfaces;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    /// <summary>
    /// Radio button renderer for boolean inputs
    /// </summary>
    public class BooleanRadioInputRenderer : IHtmlRenderer {
        /// <summary>
        /// Renders the input
        /// </summary>
        /// <param name="modelEx"></param>
        /// <param name="htmlGenerator"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="viewContext"></param>
        /// <param name="css">Custom CSS to add to the input</param>
        /// <returns></returns>
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string css, Dictionary<string, string> htmlAttributes) {
            TagHelperUtilities tagHelperUtilities = new(modelEx, htmlGenerator, htmlHelper, viewContext);
            TagBuilder output = new("div");
            var trueButton = tagHelperUtilities.CreateBooleanRadioInput(true, css, htmlAttributes);
            var falseButton = tagHelperUtilities.CreateBooleanRadioInput(false, css, htmlAttributes);


            output.InnerHtml.AppendHtml(trueButton);
            output.InnerHtml.AppendHtml(falseButton);

            if (modelEx.ModelExplorer.Metadata.IsNullableValueType) {
                var nullButton = tagHelperUtilities.CreateBooleanRadioInput(null, css, htmlAttributes);
                output.InnerHtml.AppendHtml(nullButton);
            }

            return output;
        }
    }
}
