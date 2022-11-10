using Microsoft.AspNetCore.Html;
using System.IO;
using System.Text.Encodings.Web;

namespace TowerSoft.TagHelpers.Extensions {
    internal static class HtmlContentExtensions {
        internal static string ToRawString(this IHtmlContent htmlContent) {
            using StringWriter writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
