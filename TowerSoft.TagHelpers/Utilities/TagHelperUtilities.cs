using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.HtmlRenderers;
using TowerSoft.TagHelpers.Options;

namespace TowerSoft.TagHelpers.Utilities {
    internal class TagHelperUtilities {
        internal TagHelperUtilities(ModelExpression model, IHtmlGenerator htmlGenerator,
            IHtmlHelper htmlHelper, ViewContext viewContext) {

            For = model;
            HtmlGenerator = htmlGenerator;
            HtmlHelper = htmlHelper;
            ViewContext = viewContext;
        }

        public ModelExpression For { get; }

        public IHtmlGenerator HtmlGenerator { get; }

        public IHtmlHelper HtmlHelper { get; }

        public ViewContext ViewContext { get; }

        #region Create Elements
        internal async Task<TagHelperOutput> CreateLabelElement(TagHelperContext context, string? labelName = null, string? css = null) {

            LabelTagHelper labelTagHelper =
                new LabelTagHelper(HtmlGenerator) {
                    For = For,
                    ViewContext = ViewContext
                };

            TagHelperOutput labelOutput = CreateTagHelperOutput("label");
            if (!string.IsNullOrWhiteSpace(labelName))
                labelOutput.Content.SetContent(labelName);

            if (!string.IsNullOrWhiteSpace(css))
                labelOutput.AddClass(css, HtmlEncoder.Default);

            await labelTagHelper.ProcessAsync(context, labelOutput);
            return labelOutput;
        }

        internal async Task<TagHelperOutput> CreateLabelRequiredElement(TagHelperContext context, string? labelName = null, string? css = null) {
            LabelRequiredTagHelper labelTagHelper =
                new LabelRequiredTagHelper(HtmlGenerator) {
                    For = For,
                    ViewContext = ViewContext
                };

            TagHelperOutput labelOutput = CreateTagHelperOutput("label");
            if (!string.IsNullOrWhiteSpace(labelName))
                labelOutput.Content.SetContent(labelName);

            await labelTagHelper.ProcessAsync(context, labelOutput);
            return labelOutput;
        }

        internal IHtmlContent CreateInputElement(string? rendererName = null, string? css = null) {
            if (!string.IsNullOrWhiteSpace(rendererName))
                return RendererRegistration.Get(rendererName).Render(For, HtmlGenerator, HtmlHelper, ViewContext, css);

            Type type = For.Metadata.ModelType;
            if (type != null) {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                    type = Nullable.GetUnderlyingType(type) ?? type;
                }

                if (For.Metadata.ContainerMetadata != null) {
                    PropertyInfo? prop = For.Metadata.ContainerMetadata.ModelType.GetProperty(For.Metadata.PropertyName);
                    if (prop != null && prop.IsDefined(typeof(DataTypeAttribute))) {
                        DataTypeAttribute? attr = prop.GetCustomAttribute<DataTypeAttribute>();
                        if (attr != null && RendererRegistration.Exists(attr.DataType.ToString())) {
                            return RendererRegistration.Get(attr.DataType.ToString()).Render(For, HtmlGenerator, HtmlHelper, ViewContext, css);
                        }
                    }
                }
            }

            if (type != null && RendererRegistration.Exists(type.Name)) {
                return RendererRegistration.Get(type.Name).Render(For, HtmlGenerator, HtmlHelper, ViewContext, css);
            } else {
                return RendererRegistration.Default().Render(For, HtmlGenerator, HtmlHelper, ViewContext, css);
            }
        }

        internal async Task<TagHelperOutput> CreateSelectElement(TagHelperContext context, IEnumerable<SelectListItem> items, bool multiple = false, string? optionLabel = null) {
            if (For.Model != null) {
                if (For.ModelExplorer.ModelType.IsEnum) {
                    foreach (SelectListItem item in items) {
                        if (item.Value.ToString() == ((int)For.Model).ToString()) {
                            item.Selected = true;
                        }
                    }
                } else {
                    foreach (SelectListItem item in items) {
                        if (item.Value.ToString() == For.Model.ToString()) {
                            item.Selected = true;
                        }
                    }
                }
            }

            SelectTagHelper selectTagHelper = new SelectTagHelper(HtmlGenerator) {
                For = For,
                Items = items,
                ViewContext = ViewContext
            };

            TagHelperOutput selectOutput = CreateTagHelperOutput("select");
            selectOutput.AddClass("form-select", HtmlEncoder.Default);
            if (!multiple && optionLabel != "null" || multiple && !string.IsNullOrWhiteSpace(optionLabel)) {
                TagBuilder option = new TagBuilder("option");
                option.InnerHtml.SetContent(optionLabel);
                option.Attributes.Add("value", "");
                selectOutput.PreContent.SetHtmlContent(option);
            }
            if (multiple) {
                selectOutput.Attributes.Add("multiple", "");
            }
            await selectTagHelper.ProcessAsync(context, selectOutput);
            return selectOutput;
        }

        internal async Task<TagHelperOutput> CreateValidationMessageElement(TagHelperContext context) {
            ValidationMessageTagHelper validationMessageTagHelper =
                new ValidationMessageTagHelper(HtmlGenerator) {
                    For = For,
                    ViewContext = ViewContext
                };

            TagHelperOutput validationMessageOutput = CreateTagHelperOutput("span");
            await validationMessageTagHelper.ProcessAsync(context, validationMessageOutput);
            return validationMessageOutput;
        }

        internal async Task<TagHelperOutput> CreateDescriptionElement(TagHelperContext context) {
            DescriptionTagHelper descriptionTagHelper = new DescriptionTagHelper() {
                ModelEx = For
            };

            TagHelperOutput validationMessageOutput = CreateTagHelperOutput("div");
            await descriptionTagHelper.ProcessAsync(context, validationMessageOutput);
            return validationMessageOutput;
        }
        #endregion

        #region DisplayFor TagHelper Helpers
        public static IHtmlContent TagHelperDisplay(IHtmlHelper htmlHelper, ModelExpression modelExpression, string templateName) {
            if (htmlHelper is HtmlHelper htmlHelperConcrete) {
                if (_getDisplayThunk == null) {
                    var methodInfo = typeof(HtmlHelper).GetTypeInfo().GetMethod("GenerateDisplay", BindingFlags.NonPublic | BindingFlags.Instance);
                    _getDisplayThunk = (Func<HtmlHelper, ModelExplorer, string, string, object, IHtmlContent>)methodInfo
                        .CreateDelegate(typeof(Func<HtmlHelper, ModelExplorer, string, string, object, IHtmlContent>));
                }
                return _getDisplayThunk(htmlHelperConcrete, modelExpression.ModelExplorer, GetExpressionText(modelExpression.Name), templateName, null);
            }
            return htmlHelper.Display(GetExpressionText(modelExpression.Name), templateName);
        }

        private static Func<HtmlHelper, ModelExplorer, string, string, object, IHtmlContent>? _getDisplayThunk;

        private static string GetExpressionText(string expression) {
            // If it's exactly "model", then give them an empty string, to replicate the lambda behavior.
            return string.Equals(expression, "model", StringComparison.OrdinalIgnoreCase) ? string.Empty : expression;
        }
        #endregion

        private TagHelperOutput CreateTagHelperOutput(string tagName) {
            return new TagHelperOutput(
                tagName: tagName,
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (s, t) => {
                    return Task.Factory.StartNew<TagHelperContent>(
                            () => new DefaultTagHelperContent());
                }
            );
        }
    }
}
