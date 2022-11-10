using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    /// <summary>
    /// Default renderer for boolean inputs
    /// </summary>
    public class BooleanInputRenderer : IHtmlRenderer {
        /// <summary>
        /// Renders the input
        /// </summary>
        /// <param name="modelEx"></param>
        /// <param name="htmlGenerator"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="viewContext"></param>
        /// <param name="css">Custom CSS to add to the input</param>
        /// <returns></returns>
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css) {
            TagBuilder output = htmlGenerator.GenerateCheckBox(viewContext, modelEx.ModelExplorer, modelEx.Name, (bool?)modelEx.Model, null);
            output.AddCssClass("form-check-input");
            if (!string.IsNullOrWhiteSpace(css))
                output.AddCssClass(css);

            return output;
        }
    }
}
