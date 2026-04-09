using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Options;
using TowerSoft.TagHelpers.TagHelpers.Forms;

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
        internal async Task<TagHelperOutput> CreateLabelElement(TagHelperContext context, string labelName = null, string css = null) {

            LabelTagHelper labelTagHelper =
                new(HtmlGenerator) {
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

        internal async Task<TagHelperOutput> CreateLabelRequiredElement(TagHelperContext context, string labelName = null, string css = null, bool? forceRequiredAstrix = null) {
            LabelRequiredTagHelper labelTagHelper = new(HtmlGenerator) {
                For = For,
                ViewContext = ViewContext,
                ForceRequiredAstrix = forceRequiredAstrix
            };

            TagHelperOutput labelOutput = CreateTagHelperOutput("label");
            if (!string.IsNullOrWhiteSpace(labelName))
                labelOutput.Content.SetContent(labelName);

            if (!string.IsNullOrWhiteSpace(css))
                labelOutput.AddClass(css, HtmlEncoder.Default);

            await labelTagHelper.ProcessAsync(context, labelOutput);
            return labelOutput;
        }

        internal IHtmlContent CreateInputElement(string rendererName = null, string css = null, Dictionary<string, string> htmlAttributes = null) {
            if (!string.IsNullOrWhiteSpace(rendererName))
                return RendererRegistration.Get(rendererName).Render(For, HtmlGenerator, HtmlHelper, ViewContext, css, htmlAttributes);

            Type type = For.Metadata.ModelType;
            if (type != null) {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                    type = Nullable.GetUnderlyingType(type) ?? type;
                }

                if (For.Metadata.ContainerMetadata != null) {
                    PropertyInfo prop = For.Metadata.ContainerMetadata.ModelType.GetProperty(For.Metadata.PropertyName);
                    if (prop != null && prop.IsDefined(typeof(DataTypeAttribute))) {
                        DataTypeAttribute attr = prop.GetCustomAttribute<DataTypeAttribute>();
                        if (attr != null && RendererRegistration.Exists(attr.DataType.ToString())) {
                            return RendererRegistration.Get(attr.DataType.ToString()).Render(For, HtmlGenerator, HtmlHelper, ViewContext, css, htmlAttributes);
                        }
                    }
                }
            }

            if (type != null && RendererRegistration.Exists(type.Name)) {
                return RendererRegistration.Get(type.Name).Render(For, HtmlGenerator, HtmlHelper, ViewContext, css, htmlAttributes);
            } else {
                return RendererRegistration.Default().Render(For, HtmlGenerator, HtmlHelper, ViewContext, css, htmlAttributes);
            }
        }

        internal async Task<TagHelperOutput> CreateSelectElement(TagHelperContext context, IEnumerable<SelectListItem> items, bool multiple = false,
            string optionLabel = null, Dictionary<string, string> htmlAttributes = null) {
            if (items == null)
                throw new ArgumentException("Select list items is null");

            SelectTagHelper selectTagHelper = new(HtmlGenerator) {
                For = For,
                Items = GetSelectListItems(items),
                ViewContext = ViewContext
            };

            TagHelperOutput selectOutput = CreateTagHelperOutput("select");

            if (TowerSoftTagHelperSettings.SelectDefaultClass != null)
                selectOutput.AddClass(TowerSoftTagHelperSettings.SelectDefaultClass, HtmlEncoder.Default);

            if (!multiple && optionLabel != "null" || multiple && !string.IsNullOrWhiteSpace(optionLabel)) {
                TagBuilder option = new("option");
                option.InnerHtml.SetContent(optionLabel);
                option.Attributes.Add("value", "");
                selectOutput.PreContent.SetHtmlContent(option);
            }

            if (multiple) {
                selectOutput.Attributes.Add("multiple", "");
            }

            if (htmlAttributes != null) {
                foreach (KeyValuePair<string, string> keyValuePair in htmlAttributes) {
                    if (selectOutput.Attributes.ContainsName(keyValuePair.Key)) {
                        selectOutput.Attributes.SetAttribute(keyValuePair.Key, keyValuePair.Value);
                    } else {
                        selectOutput.Attributes.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
            await selectTagHelper.ProcessAsync(context, selectOutput);
            return selectOutput;
        }

        internal async Task<TagHelperOutput> CreateValidationMessageElement(TagHelperContext context) {
            ValidationMessageTagHelper validationMessageTagHelper = new(HtmlGenerator) {
                For = For,
                ViewContext = ViewContext
            };

            TagHelperOutput validationMessageOutput = CreateTagHelperOutput("span");
            await validationMessageTagHelper.ProcessAsync(context, validationMessageOutput);
            return validationMessageOutput;
        }

        internal async Task<TagHelperOutput> CreateDescriptionElement(TagHelperContext context) {
            DescriptionTagHelper descriptionTagHelper = new() {
                ModelEx = For
            };

            TagHelperOutput validationMessageOutput = CreateTagHelperOutput("div");
            await descriptionTagHelper.ProcessAsync(context, validationMessageOutput);
            return validationMessageOutput;
        }

        internal TagBuilder CreateBooleanRadioInput(bool? value, string css, Dictionary<string, string> htmlAttributes) {
            TagBuilder label = new("label");
            if (TowerSoftTagHelperSettings.CheckboxLabelClass != null)
                label.AddCssClass(TowerSoftTagHelperSettings.CheckboxLabelClass);

            TagBuilder input = HtmlGenerator.GenerateRadioButton(ViewContext, For.ModelExplorer, For.Name, value, (bool?)For.Model == value, null);

            if (TowerSoftTagHelperSettings.CheckboxInputClass != null)
                input.AddCssClass(TowerSoftTagHelperSettings.CheckboxInputClass);

            if (!string.IsNullOrWhiteSpace(css))
                input.Attributes["class"] = css;

            if (value == null && For.Model == null && !input.Attributes.ContainsKey("checked")) {
                input.Attributes.Add("checked", "");
            }

            string trueLabelText = "Yes";
            string falseLabelText = "No";
            string nullLabelText = "Not Set";
            if (htmlAttributes != null && htmlAttributes.TryGetValue("data-labels", out string labels)) {
                string[] parts = labels.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 1) {
                    trueLabelText = parts[0].Trim();
                }
                if (parts.Length >= 2) {
                    falseLabelText = parts[1].Trim();
                }
                if (parts.Length >= 3) {
                    nullLabelText = parts[2].Trim();
                }
            }


            label.InnerHtml.AppendHtml(input);
            if (value.HasValue && value.Value)
                label.InnerHtml.Append(trueLabelText);
            else if (value.HasValue)
                label.InnerHtml.Append(falseLabelText);
            else
                label.InnerHtml.Append(nullLabelText);

            if (TowerSoftTagHelperSettings.CheckboxInlineContainerClass != null) {
                TagBuilder div = new("div");
                div.AddCssClass(TowerSoftTagHelperSettings.CheckboxInlineContainerClass);
                div.InnerHtml.AppendHtml(label);
                return div;
            }

            return label;
        }
        #endregion

        #region DisplayFor TagHelper Helpers
        public static IHtmlContent TagHelperDisplay(IHtmlHelper htmlHelper, ModelExpression modelExpression, string templateName) {
            if (htmlHelper is HtmlHelper htmlHelperConcrete) {
                if (_getDisplayThunk == null) {
                    MethodInfo methodInfo = typeof(HtmlHelper).GetTypeInfo().GetMethod("GenerateDisplay", BindingFlags.NonPublic | BindingFlags.Instance);
                    _getDisplayThunk = (Func<HtmlHelper, ModelExplorer, string, string, object, IHtmlContent>)methodInfo
                        .CreateDelegate(typeof(Func<HtmlHelper, ModelExplorer, string, string, object, IHtmlContent>));
                }
                return _getDisplayThunk(htmlHelperConcrete, modelExpression.ModelExplorer, GetExpressionText(modelExpression.Name), templateName, null);
            }
            return htmlHelper.Display(GetExpressionText(modelExpression.Name), templateName);
        }

        private static Func<HtmlHelper, ModelExplorer, string, string, object, IHtmlContent> _getDisplayThunk;

        private static string GetExpressionText(string expression) {
            // If it's exactly "model", then give them an empty string, to replicate the lambda behavior.
            return string.Equals(expression, "model", StringComparison.OrdinalIgnoreCase) ? string.Empty : expression;
        }
        #endregion

        private static TagHelperOutput CreateTagHelperOutput(string tagName) {
            return new TagHelperOutput(
                tagName: tagName,
                attributes: [],
                getChildContentAsync: (s, t) => {
                    return Task.Factory.StartNew<TagHelperContent>(
                            () => new DefaultTagHelperContent());
                }
            );
        }

        private List<SelectListItem> GetSelectListItems(IEnumerable<SelectListItem> items) {
            List<SelectListItem> output = [];
            if (For.ModelExplorer.ModelType.IsEnum) {
                foreach (SelectListItem item in items) {
                    output.Add(GetSelectListItem(item, ((int)For.Model).ToString()));
                }
            } else {
                IEnumerable modelList = null;
                if (For.Model != null) {
                    modelList = For.Model as IEnumerable;
                }

                if (modelList == null || For.Model is string) {
                    foreach (SelectListItem item in items) {
                        output.Add(GetSelectListItem(item, For?.Model?.ToString()));
                    }
                } else {
                    List<string> modelStrings = [];
                    foreach (object listValue in modelList) {
                        modelStrings.Add(listValue.ToString());
                    }
                    foreach (SelectListItem item in items) {
                        output.Add(GetSelectListItem(item, compareList: modelStrings));
                    }
                }
            }
            return output;
        }

        private static SelectListItem GetSelectListItem(SelectListItem item, string compareTo = null, IEnumerable<string> compareList = null) {
            SelectListItem newItem = new() {
                Text = item.Text,
                Value = item.Value,
                Group = item.Group,
                Disabled = item.Disabled
            };

            string compareFrom = item.Text;
            if (item.Value != null)
                compareFrom = item.Value;

            if (compareList != null) {
                if (compareList.Contains(compareFrom))
                    newItem.Selected = true;
            } else if (compareTo == compareFrom) {
                newItem.Selected = true;
            }
            return newItem;
        }
    }
}
