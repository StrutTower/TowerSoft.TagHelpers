using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    public class DateTimeInputRenderer : IHtmlRenderer {
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string css, Dictionary<string, string> htmlAttributes) {
            string value = string.Empty;
            if (modelEx.Model != null && modelEx.Model is DateTime datetime) {
                if (datetime != DateTime.MinValue)
                    value = ((DateTime)modelEx.Model).ToString("yyyy-MM-dd HH:mm:ss");
            }

            TagBuilder output = htmlGenerator.GenerateTextBox(viewContext, modelEx.ModelExplorer, modelEx.Name, value, null, new { type = "datetime-local" });

            output.AddCssClass("form-control");
            if (!string.IsNullOrWhiteSpace(css))
                output.Attributes["class"] = css;

            if (htmlAttributes != null) {
                output.MergeAttributes(htmlAttributes);
            }

            return output;
        }
    }
}
