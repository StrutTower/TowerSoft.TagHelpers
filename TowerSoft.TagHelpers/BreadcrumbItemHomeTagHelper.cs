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

namespace TowerSoft.TagHelpers {
    /// <summary>
    /// Generates a Bootstrap breadcrumb item that default to the homepage of the application.
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="urlHelperFactory"></param>
    [HtmlTargetElement("breadcrumb-item-home", ParentTag = "breadcrumbs", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BreadcrumbItemHomeTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) : TagHelper {
        private IDictionary<string, string> _routeValues;

        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string AreaAttributeName = "asp-area";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "asp-route-";
        private const string FragmentAttributeName = "asp-fragment";


        /// <summary>Optional. Action name for the link. Exclude this to render it as the active item.</summary>
        /// <remarks>Default is Index</remarks>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>Optional. Controller name for the link.</summary>
        /// <remarks>Default is Home</remarks>
        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        /// <summary>Optional. Area name where the controller is located.</summary>
        /// <remarks>Default is ""</remarks>
        [HtmlAttributeName(AreaAttributeName)]
        public string Area { get; set; }

        /// <summary>Optional. Route values for the link</summary>
        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }

        /// <summary>Optional. URL Fragment</summary>
        [HtmlAttributeName(FragmentAttributeName)]
        public string Fragment { get; set; }

        /// <summary>Optional. Route values for the link</summary>
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

        /// <summary></summary>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            (htmlHelper as IViewContextAware).Contextualize(ViewContext);
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            string action = Action ?? "Index";
            string controller = Controller ?? "Home";
            string fragment = Fragment ?? null;

            RouteValueDictionary routeValues = [];
            if (_routeValues != null && _routeValues.Count > 0) {
                routeValues = new RouteValueDictionary(_routeValues);
            }
            routeValues["area"] = Area ?? "";

            TagBuilder link = new("a");
            link.Attributes.Add("href", urlHelper.Action(action, controller, routeValues, null, null, fragment: fragment));

            TagHelperContent childContent = await output.GetChildContentAsync();
            if (childContent.IsEmptyOrWhiteSpace) {
                TagBuilder icon = new("span");

                icon.AddCssClass("mdi mdi-fw mdi-home");

                TagBuilder hiddenText = new("span");
                hiddenText.AddCssClass("visually-hidden");
                hiddenText.InnerHtml.Append("Home");

                link.InnerHtml.AppendHtml(icon);
                link.InnerHtml.AppendHtml(hiddenText);
            } else {
                link.InnerHtml.AppendHtml(childContent);
            }

            output.TagName = "li";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("breadcrumb-item", HtmlEncoder.Default);
            output.Content.AppendHtml(link);
        }
    }
}
