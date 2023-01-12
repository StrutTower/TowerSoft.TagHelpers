using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TowerSoft.TagHelpers {
    [HtmlTargetElement("hrFormSubmit")]
    public class HorizontalFormSubmitTagHelper : TagHelper {
        public string LabelCol { get; set; }

        public string InputCol { get; set; }

        public string ButtonClass { get; set; }

        public string ButtonText { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            string labelColumnCss = LabelCol ?? "col-md-4 col-lg-3";
            string fieldColumnCss = InputCol ?? "col-md-7 col-lg-6";
            string buttonClass = ButtonClass ?? "btn btn-primary";
            string buttonText = ButtonText ?? "Save";

            TagBuilder button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.AddCssClass(buttonClass);
            button.InnerHtml.Append(buttonText);

            TagBuilder label = new TagBuilder("div");
            label.AddCssClass(labelColumnCss);

            TagBuilder field = new TagBuilder("div");
            field.AddCssClass(fieldColumnCss);
            field.InnerHtml.SetHtmlContent(button);

            field.InnerHtml.AppendHtml(await output.GetChildContentAsync());

            output.TagName = "div";
            output.AddClass("row", HtmlEncoder.Default);
            output.AddClass("mb-3", HtmlEncoder.Default);
            output.Content.AppendHtml(label);
            output.Content.AppendHtml(field);
        }
    }
}
