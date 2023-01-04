using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    [HtmlTargetElement("hrFormField", Attributes = "asp-for")]
    public class HorizontalFormFieldTagHelper : TagHelper {
        public HorizontalFormFieldTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper) {
            HtmlGenerator = htmlGenerator;
            HtmlHelper = htmlHelper;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public string? Template { get; set; }

        public string? Label { get; set; }

        public string? LabelCol { get; set; }

        public string? InputCol { get; set; }

        public string? InputCss { get; set; }

        public IHtmlGenerator HtmlGenerator { get; }

        public IHtmlHelper HtmlHelper { get; }

        /// <summary></summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            ((IViewContextAware)HtmlHelper).Contextualize(ViewContext);

            output.TagName = "div";
            output.AddClass("row", HtmlEncoder.Default);
            output.AddClass("mb-3", HtmlEncoder.Default);

            TagHelperUtilities utils = new TagHelperUtilities(For, HtmlGenerator, HtmlHelper, ViewContext);

            Type type = For.Metadata.ModelType;
            type = Nullable.GetUnderlyingType(type) ?? type;

            string labelColumnCss = LabelCol ?? "col-md-4 col-lg-3";
            string fieldColumnCss = InputCol ?? "col-md-7 col-lg-6";

            if (type == typeof(bool) && string.IsNullOrWhiteSpace(Template)) {
                TagBuilder labelDiv = new TagBuilder("div");
                labelDiv.AddCssClass(labelColumnCss);
                TagBuilder fieldDiv = new TagBuilder("div");
                fieldDiv.AddCssClass(fieldColumnCss);

                // Handle Booleans/Checkbox
                TagBuilder formCheck = new TagBuilder("div");
                formCheck.AddCssClass("form-check");
                formCheck.InnerHtml.AppendHtml(utils.CreateInputElement(null, InputCss));
                formCheck.InnerHtml.AppendHtml(await utils.CreateLabelElement(context, Label, "form-check-label"));
                fieldDiv.InnerHtml.AppendHtml(formCheck);
                fieldDiv.InnerHtml.AppendHtml(await utils.CreateValidationMessageElement(context));
                fieldDiv.InnerHtml.AppendHtml(await utils.CreateDescriptionElement(context));

                output.Content.AppendHtml(labelDiv);
                output.Content.AppendHtml(fieldDiv);
            } else {
                TagBuilder labelDiv = new TagBuilder("div");
                labelDiv.AddCssClass(labelColumnCss + " text-md-end");
                TagBuilder fieldDiv = new TagBuilder("div");
                fieldDiv.AddCssClass(fieldColumnCss);

                // Default editor or supplied editor template
                TagHelperOutput labelElement = await utils.CreateLabelRequiredElement(context, Label);
                IHtmlContent inputElement = utils.CreateInputElement(Template, InputCss);
                TagHelperOutput validationMessageElement = await utils.CreateValidationMessageElement(context);
                TagHelperOutput descriptionElement = await utils.CreateDescriptionElement(context);

                labelDiv.InnerHtml.AppendHtml(labelElement);
                fieldDiv.InnerHtml.AppendHtml(inputElement);
                fieldDiv.InnerHtml.AppendHtml(validationMessageElement);
                fieldDiv.InnerHtml.AppendHtml(descriptionElement);

                output.Content.AppendHtml(labelDiv);
                output.Content.AppendHtml(fieldDiv);
            }
        }
    }
}
