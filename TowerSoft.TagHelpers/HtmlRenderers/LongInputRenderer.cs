using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    public class LongInputRenderer : IHtmlRenderer {
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string css, Dictionary<string, string> htmlAttributes) {
            string value = string.Empty;
            if (modelEx.Model != null && modelEx.Model is long numberLong && numberLong != 0) {
                value = numberLong.ToString();
            }
            if (modelEx.Model != null && modelEx.Model is int numberInt && numberInt != 0) {
                value = numberInt.ToString();
            }

            TagBuilder output = htmlGenerator.GenerateTextBox(viewContext, modelEx.ModelExplorer, modelEx.Name, value, null, new { type = "number" });
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
