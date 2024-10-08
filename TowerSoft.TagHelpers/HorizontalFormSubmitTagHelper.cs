﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Creates a submit button with the horizontal layout. Additional elements can be rendered next to the button from the inner HTML.
    /// </summary>
    [HtmlTargetElement("hrFormSubmit")]
    public class HorizontalFormSubmitTagHelper : TagHelper {
        /// <summary>
        /// Bootstrap column CSS for the label. If not set, defaults to: col-md-4 col-lg-3
        /// </summary>
        public string LabelCol { get; set; }

        /// <summary>
        /// Bootstrap column CSS for the input. If not set, defaults to: col-md-7 col-lg-6
        /// </summary>
        public string InputCol { get; set; }

        /// <summary>
        /// Sets the CSS class on the button. Default 'btn btn-primary'
        /// </summary>
        public string ButtonClass { get; set; }

        /// <summary>
        /// Sets the text of the button. Default 'Save'
        /// </summary>
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

            TagBuilder button = new("button");
            button.Attributes.Add("type", "submit");
            button.AddCssClass(buttonClass);
            button.InnerHtml.Append(buttonText);

            TagBuilder label = new("div");
            label.AddCssClass(labelColumnCss);

            TagBuilder field = new("div");
            field.AddCssClass(fieldColumnCss);
            field.InnerHtml.SetHtmlContent(button);

            field.InnerHtml.AppendHtml(await output.GetChildContentAsync());

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("row", HtmlEncoder.Default);
            output.AddClass("mb-3", HtmlEncoder.Default);
            output.Content.AppendHtml(label);
            output.Content.AppendHtml(field);
        }
    }
}
