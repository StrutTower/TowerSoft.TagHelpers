using Microsoft.AspNetCore.Html;

namespace TowerSoft.TagHelpers.TagHelpers.Modals {
    internal class BootstrapModalContext {
        public IHtmlContent Body { get; set; }

        public string BodyClass { get; set; }

        public IHtmlContent Footer { get; set; }

        public string FooterClass { get; set; }
    }
}
