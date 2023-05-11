using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpersTests.HtmlRenderers {
    public class TestRenderer : IHtmlRenderer {
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css, Dictionary<string, string>? htmlAttributes) {
            return new StringHtmlContent("test");
        }
    }
}
