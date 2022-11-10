using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Renders the description of the property from the DescriptionAttribute or DisplayAttribtue Description
    /// </summary>
    [HtmlTargetElement("description", Attributes = "asp-for")]
    public class DescriptionTagHelper : TagHelper {

        /// <summary>
        /// Model expression for the property
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression ModelEx { get; set; }

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "div";
            output.Attributes.Add("class", "form-help-text");
            var prop = ModelEx.Metadata.ContainerType.GetProperty(ModelEx.Name);
            if (prop != null) {
                DescriptionAttribute? descAttr = prop.GetCustomAttribute<DescriptionAttribute>(true);
                if (descAttr != null) {
                    output.Content.SetContent(descAttr.Description);
                } else {
                    DisplayAttribute? disAttr = prop.GetCustomAttribute<DisplayAttribute>(true);
                    if (disAttr != null) {
                        output.Content.SetContent(disAttr.Description);
                    }
                }
            }
        }
    }
}
