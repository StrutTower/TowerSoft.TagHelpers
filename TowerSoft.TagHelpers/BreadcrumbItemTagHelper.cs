using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TowerSoft.TagHelpers.Extensions;
using TowerSoft.TagHelpers.Utilities;

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Generates a Bootstrap breadcrumb item with action, controller, and other attributes for creating links.
    /// Excluding the action attribute will render it as the active item.
    /// </summary>
    [HtmlTargetElement("breadcrumb-item", ParentTag = "breadcrumbs")]
    [HtmlTargetElement("breadcrumb-item", Attributes = ActionAttributeName, ParentTag = "breadcrumbs")]
    [HtmlTargetElement("breadcrumb-item", Attributes = ControllerAttributeName, ParentTag = "breadcrumbs")]
    [HtmlTargetElement("breadcrumb-item", Attributes = AreaAttributeName, ParentTag = "breadcrumbs")]
    [HtmlTargetElement("breadcrumb-item", Attributes = RouteValuesPrefix + "*", ParentTag = "breadcrumbs")]
    [HtmlTargetElement("breadcrumb-item", Attributes = DisplayForAttributeName, ParentTag = "breadcrumbs")]
    [HtmlTargetElement("breadcrumb-item", Attributes = TemplateAttributeName, ParentTag = "breadcrumbs")]
    public class BreadcrumbItemTagHelper : TagHelper {
        private readonly IHtmlHelper htmlHelper;
        private readonly IUrlHelperFactory UrlHelperFactory;
        private IDictionary<string, string> _routeValues;

        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string AreaAttributeName = "asp-area";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "asp-route-";
        private const string DisplayForAttributeName = "display-for";
        private const string TemplateAttributeName = "template";

        /// <summary>
        /// BreadcrumbItem constructor
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="urlHelperFactory"></param>
        public BreadcrumbItemTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) {
            this.htmlHelper = htmlHelper;
            UrlHelperFactory = urlHelperFactory;
        }

        /// <summary>
        /// Action name for the link. Exclude this to render it as the active item.
        /// </summary>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// Controller name for the link
        /// </summary>
        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        /// <summary>
        /// Area name where the controller is located
        /// </summary>
        [HtmlAttributeName(AreaAttributeName)]
        public string Area { get; set; }

        /// <summary>
        /// Route values for the link
        /// </summary>
        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }

        /// <summary>
        /// Model expression for the property
        /// </summary>
        [HtmlAttributeName(DisplayForAttributeName)]
        public ModelExpression? Model { get; set; }

        /// <summary>
        /// Name of the display template
        /// </summary>
        [HtmlAttributeName(TemplateAttributeName)]
        public string TemplateName { get; set; }

        /// <summary>
        /// Route values for the link
        /// </summary>
        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues {
            get {
                if (_routeValues == null) {
                    _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                return _routeValues;
            }
            set {
                _routeValues = value;
            }
        }

        /// <summary></summary>
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Process method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            (htmlHelper as IViewContextAware).Contextualize(ViewContext);
            IUrlHelper urlHelper = UrlHelperFactory.GetUrlHelper(ViewContext);

            RouteValueDictionary? routeValues = null;
            if (_routeValues != null && _routeValues.Count > 0) {
                routeValues = new RouteValueDictionary(_routeValues);
            }

            if (Area != null) {
                if (routeValues == null) routeValues = new RouteValueDictionary();
                routeValues["area"] = Area;
            }

            if (string.IsNullOrWhiteSpace(Action)) {
                output.TagName = "li";
                output.AddClass("breadcrumb-item", HtmlEncoder.Default);
                output.AddClass("active", HtmlEncoder.Default);
            } else {
                TagBuilder a = new TagBuilder("a");
                a.Attributes.Add("href", urlHelper.Action(Action, Controller, routeValues));

                if (Model != null) {
                    a.InnerHtml.AppendHtml(TagHelperUtilities.TagHelperDisplay(htmlHelper, Model, TemplateName));
                }

                a.InnerHtml.AppendHtml((await output.GetChildContentAsync()).GetContent());

                output.TagName = "li";
                output.AddClass("breadcrumb-item", HtmlEncoder.Default);
                output.Content.SetHtmlContent(a.ToRawString());
            }
        }
    }
}
