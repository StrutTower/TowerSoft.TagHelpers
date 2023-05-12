using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.HtmlRenderers {
    public class EmailInputHtmlRenderer : IHtmlRenderer {
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css, Dictionary<string, string>? htmlAttributes) {
            TagBuilder output = htmlGenerator.GenerateTextBox(viewContext, modelEx.ModelExplorer, modelEx.Name, modelEx.Model, null, null);
            output.Attributes["type"] = "email";
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
