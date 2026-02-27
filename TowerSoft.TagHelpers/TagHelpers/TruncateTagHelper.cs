using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Displays a string and will truncate it to the number of max characters
    /// </summary>
    public class TruncateTagHelper : TagHelper {
        /// <summary>
        /// String to display
        /// </summary>
        public string String { get; set; }

        /// <summary>
        /// Maximum number of character to display
        /// </summary>
        public int Characters { get; set; }

        /// <summary>
        /// Process Method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = null;
            string outputString = String;
            if (String.Length > Characters) {
                outputString = String[..Characters] + "...";
            }
            output.Content.SetContent(outputString);
        }
    }
}
