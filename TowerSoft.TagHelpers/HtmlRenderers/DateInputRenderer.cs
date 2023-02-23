using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    public class DateInputRenderer : IHtmlRenderer {
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css, Dictionary<string, string>? htmlAttributes) {
            string value = string.Empty;
            if (modelEx.Model != null && modelEx.Model is DateTime date) {
                if (date != DateTime.MinValue)
                    value = ((DateTime)modelEx.Model).ToString("yyyy-MM-dd");
            }

            TagBuilder output = htmlGenerator.GenerateTextBox(viewContext, modelEx.ModelExplorer, modelEx.Name, value, null, new { type = "date" });

            output.AddCssClass("form-control");
            if (!string.IsNullOrWhiteSpace(css))
                output.AddCssClass(css);

            if (htmlAttributes != null) {
                output.MergeAttributes(htmlAttributes);
            }

            return output;
        }
    }
}
