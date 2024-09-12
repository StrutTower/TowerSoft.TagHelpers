using Microsoft.AspNetCore.Razor.TagHelpers;

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
