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
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css, Dictionary<string, string> htmlAttributes) {
            TagBuilder output = new("div");
            var trueButton = GetFormCheckInput(true, modelEx, htmlGenerator, viewContext, css, htmlAttributes);
            var falseButton = GetFormCheckInput(false, modelEx, htmlGenerator, viewContext, css, htmlAttributes);

            output.InnerHtml.AppendHtml(trueButton);
            output.InnerHtml.AppendHtml(falseButton);

            if (modelEx.ModelExplorer.Metadata.IsNullableValueType) {
                var nullButton = GetFormCheckInput(null, modelEx, htmlGenerator, viewContext, css, htmlAttributes);
                output.InnerHtml.AppendHtml(nullButton);
            }

            return output;
        }

        private TagBuilder GetFormCheckInput(bool? value, ModelExpression modelEx, IHtmlGenerator htmlGenerator, ViewContext viewContext, string css, Dictionary<string, string> htmlAttributes) {
            TagBuilder div = new("div");
            div.AddCssClass("form-check form-check-inline");

            TagBuilder label = new("label");
            label.AddCssClass("form-check-label");

            TagBuilder input = htmlGenerator.GenerateRadioButton(viewContext, modelEx.ModelExplorer, modelEx.Name, value, (bool?)modelEx.Model == value, null);
            input.AddCssClass("form-check-input");
            if (!string.IsNullOrWhiteSpace(css))
                input.Attributes["class"] = css;

            if (value == null && modelEx.Model == null && !input.Attributes.ContainsKey("checked")) {
                input.Attributes.Add("checked", "");
            }

            string trueLabelText = "Yes";
            string falseLabelText = "No";
            string nullLabelText = "Not Set";
            if (htmlAttributes != null && htmlAttributes.ContainsKey("data-labels")) {
                string labels = htmlAttributes["data-labels"];
                string[] parts = labels.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 1) {
                    trueLabelText = parts[0].Trim();
                }
                if (parts.Length >= 2) {
                    falseLabelText = parts[1].Trim();
                }
                if (parts.Length >= 3) {
                    nullLabelText = parts[2].Trim();
                }
            }

            label.InnerHtml.AppendHtml(input);
            if (value.HasValue && value.Value)
                label.InnerHtml.Append(trueLabelText);
            else if (value.HasValue)
                label.InnerHtml.Append(falseLabelText);
            else
                label.InnerHtml.Append(nullLabelText);
            div.InnerHtml.AppendHtml(label);
            return div;
        }
    }
}
