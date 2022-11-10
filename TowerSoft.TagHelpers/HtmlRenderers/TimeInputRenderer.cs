using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    /// <summary>
    /// Renderer for time inputs
    /// </summary>
    public class TimeInputRenderer : IHtmlRenderer {
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
            string value = string.Empty;
            if (modelEx.Model != null && modelEx.Model is DateTime time) {
                if (time != DateTime.MinValue)
                    value = ((DateTime)modelEx.Model).ToString("HH:mm:ss");
            }

            TagBuilder output = htmlGenerator.GenerateTextBox(viewContext, modelEx.ModelExplorer, modelEx.Name, value, null, new { type = "time" });

            output.AddCssClass("form-control");
            if (!string.IsNullOrWhiteSpace(css))
                output.AddCssClass(css);

            return output;
        }
    }
}
