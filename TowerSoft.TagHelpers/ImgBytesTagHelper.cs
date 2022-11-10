using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Converts a byte[] to Base64 and displays an img element
    /// </summary>
    [HtmlTargetElement("img", Attributes = "asp-bytes")]
    public class ImgBytesTagHelper : TagHelper {
        /// <summary>
        /// Byte[] of the image data
        /// </summary>
        [HtmlAttributeName("asp-bytes")]
        public ModelExpression Data { get; set; }

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (Data.Model != null && Data.Model is byte[] data) {
                output.TagName = "img";
                output.Attributes.Add("src", "data:image/jpg;base64," + Convert.ToBase64String(data));
            }
        }
    }
}
