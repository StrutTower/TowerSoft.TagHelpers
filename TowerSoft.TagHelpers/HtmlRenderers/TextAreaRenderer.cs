﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    /// <summary>
    /// Render for textarea inputs
    /// </summary>
    public class TextAreaRenderer : IHtmlRenderer {
        /// <summary>
        /// Renders the element
        /// </summary>
        /// <param name="modelEx"></param>
        /// <param name="htmlGenerator"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="viewContext"></param>
        /// <param name="css">Custom CSS to add to the input</param>
        /// <returns></returns>
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css) {
            TagBuilder output = htmlGenerator.GenerateTextArea(viewContext, modelEx.ModelExplorer, modelEx.Name, 6, 10, null);
            output.AddCssClass("form-control");

            if (!string.IsNullOrWhiteSpace(css))
                output.AddCssClass(css);

            return output;
        }
    }
}
