using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Buffers;
using System.Diagnostics;
using System.Text.Encodings.Web;

namespace TowerSoft.TagHelpersTests.Utilities {
    public static class TagHelperUtils {
        public static TagHelperContext GetContext(string tagName = null) {
            return new(tagName, [], new Dictionary<object, object>(), "test");
        }

        public static TagHelperOutput GetOutput(string tagName = null) {
            return new(tagName, [], getChildContentAsync: (s, t) => {
                return Task.Factory.StartNew<TagHelperContent>(() => new DefaultTagHelperContent());
            });
        }


    }
}
