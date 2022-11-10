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
    public class FormFileInputRenderer : IHtmlRenderer {
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css) {
            TagBuilder output = new TagBuilder("input");
            output.AddCssClass("form-control");
            output.Attributes.Add("type", "file");
            output.Attributes.Add("name", htmlHelper.Name(modelEx.Name));
            output.Attributes.Add("id", htmlHelper.Id(modelEx.Name));
            output.Attributes.Add("data-val", "true");

            if (modelEx.Metadata.IsRequired) {
                output.Attributes.Add("data-val-required", "You must select a file to upload.");
            }
            return output;
        }
    }
}
