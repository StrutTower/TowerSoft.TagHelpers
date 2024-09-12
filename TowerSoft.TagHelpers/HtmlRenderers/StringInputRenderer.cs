using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    /// <summary>
    /// Renderer for <c>&lt;input type="text" /&gt;</c>
    /// </summary>
    public class StringInputRenderer : IHtmlRenderer {
        /// <summary>
        /// Renders the input
        /// </summary>
        /// <param name="modelEx">Model expression</param>
        /// <param name="htmlGenerator">IHtmlGenerator</param>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="viewContext">ViewContext</param>
        /// <param name="css">Optional. CSS classes that will be added to the input. Defaults to 'form-control'</param>
        /// <param name="htmlAttributes">Optional. Dictionary of HTML attributes</param>
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string css, Dictionary<string, string> htmlAttributes) {
            TagBuilder output = htmlGenerator.GenerateTextBox(viewContext, modelEx.ModelExplorer, modelEx.Name, modelEx.Model, null, null);
            output.AddCssClass("form-control");
            if (!string.IsNullOrWhiteSpace(css))
                output.Attributes["class"] = css;

            if (htmlAttributes != null) {
                output.MergeAttributes(htmlAttributes, true);
            }

            return output;
        }
    }
}
