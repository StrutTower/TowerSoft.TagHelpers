using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using TowerSoft.TagHelpers.Interfaces;

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
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css, Dictionary<string, string>? htmlAttributes) {
            TagBuilder output = new TagBuilder("div");
            var trueButton = GetFormCheckInput(true, modelEx, htmlGenerator, viewContext);
            var falseButton = GetFormCheckInput(false, modelEx, htmlGenerator, viewContext);

            output.InnerHtml.AppendHtml(trueButton);
            output.InnerHtml.AppendHtml(falseButton);

            if (modelEx.ModelExplorer.Metadata.IsNullableValueType) {
                var nullButton = GetFormCheckInput(null, modelEx, htmlGenerator, viewContext);
                output.InnerHtml.AppendHtml(nullButton);
            }

            return output;
        }

        private TagBuilder GetFormCheckInput(bool? value, ModelExpression modelEx, IHtmlGenerator htmlGenerator, ViewContext viewContext) {
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("form-check form-check-inline");

            TagBuilder label = new TagBuilder("label");
            label.AddCssClass("form-check-label");

            TagBuilder input = htmlGenerator.GenerateRadioButton(viewContext, modelEx.ModelExplorer, modelEx.Name, value, (bool?)modelEx.Model == value, null);
            input.AddCssClass("form-check-input");

            if (value == null && modelEx.Model == null) {
                input.Attributes.Add("checked", "");
            }

            label.InnerHtml.AppendHtml(input);
            if (value.HasValue && value.Value)
                label.InnerHtml.Append("True");
            else if (value.HasValue)
                label.InnerHtml.Append("False");
            else
                label.InnerHtml.Append("Unknown");
            div.InnerHtml.AppendHtml(label);
            return div;
        }
    }
}
