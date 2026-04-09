using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web;
using TowerSoft.TagHelpers.Interfaces;
using TowerSoft.TagHelpers.Options;
using TowerSoft.TagHelpers.Utilities;

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
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public IHtmlContent Render(ModelExpression modelEx, IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper, ViewContext viewContext, string css, Dictionary<string, string> htmlAttributes) {
            PropertyInfo prop = modelEx.Metadata.ContainerType.GetProperty(modelEx.Metadata.Name);
            bool required = false;
            bool nullable = modelEx.ModelExplorer.Metadata.IsNullableValueType;

            TagHelperUtilities utils = new(modelEx, htmlGenerator, htmlHelper, viewContext);

            if (prop != null)
                required = prop.IsDefined(typeof(RequiredAttribute), true);

            if (required || !nullable) {
                TagBuilder output = htmlGenerator.GenerateCheckBox(viewContext, modelEx.ModelExplorer, modelEx.Name, (bool?)modelEx.Model, htmlAttributes);

                if (!string.IsNullOrWhiteSpace(css))
                    output.Attributes["class"] = css;

                if (TowerSoftTagHelperSettings.CheckboxInputClass != null)
                    output.AddCssClass(TowerSoftTagHelperSettings.CheckboxInputClass);

                return output;
            } else {
                TagHelperUtilities tagHelperUtilities = new(modelEx, htmlGenerator, htmlHelper, viewContext);
                TagBuilder output = new("div");
                // Yes,No,Not Set Radio Buttons
                TagBuilder trueButton = tagHelperUtilities.CreateBooleanRadioInput(true, css, htmlAttributes);
                TagBuilder falseButton = tagHelperUtilities.CreateBooleanRadioInput(false, css, htmlAttributes);
                TagBuilder nullButton = tagHelperUtilities.CreateBooleanRadioInput(null, css, htmlAttributes);

                output.InnerHtml.AppendHtml(trueButton);
                output.InnerHtml.AppendHtml(falseButton);
                output.InnerHtml.AppendHtml(nullButton);
                return output;
            }
        }
    }
}
