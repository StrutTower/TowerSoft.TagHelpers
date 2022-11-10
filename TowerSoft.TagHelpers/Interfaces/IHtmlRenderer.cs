using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TowerSoft.TagHelpers.Interfaces {
    /// <summary>
    /// HtmlRender interfaces for creating custom renders used with the FormField and HorizontalFormField TagHelpers
    /// </summary>
    public interface IHtmlRenderer {
        /// <summary>
        /// Renders the element
        /// </summary>
        /// <param name="modelEx"></param>
        /// <param name="htmlGenerator"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="viewContext"></param>
        /// <param name="css">Custom CSS for the input element passed through from the input-css attribute</param>
        /// <returns></returns>
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string? css);
    }
}
