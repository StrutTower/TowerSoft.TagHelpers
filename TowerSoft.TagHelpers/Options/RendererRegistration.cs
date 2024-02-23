using System;
using System.Collections.Generic;
using System.Linq;
using TowerSoft.TagHelpers.HtmlRenderers;
using TowerSoft.TagHelpers.Interfaces;

namespace TowerSoft.TagHelpers.Options {
    /// <summary>
    /// Handles HtmlRenderers for the FormField and HorizontalFormField TagHelpers
    /// </summary>
    public static class RendererRegistration {
        private static Dictionary<string, IHtmlRenderer> Renderers { get; set; } = new Dictionary<string, IHtmlRenderer>();

        /// <summary>
        /// Registers all of the HtmlRenders included in the project
        /// </summary>
        public static void RegisterDefaultRenderers() {
            Add<BooleanInputRenderer>(HtmlRenderer.Boolean);
            Add<BooleanRadioInputRenderer>(HtmlRenderer.BooleanRadio);
            Add<YesNoBooleanHtmlRenderer>(HtmlRenderer.YesNoRadio);
            Add<DateInputRenderer>(HtmlRenderer.Date);
            Add<DateTimeInputRenderer>(HtmlRenderer.DateTime);
            Add<EmailInputHtmlRenderer>(HtmlRenderer.Email);
            Add<FormFileInputRenderer>(HtmlRenderer.File);
            Add<IntInputRenderer>(HtmlRenderer.Int);
            Add<LongInputRenderer>(HtmlRenderer.Long);
            Add<StringInputRenderer>(HtmlRenderer.String);
            Add<TextAreaRenderer>(HtmlRenderer.TextArea);
            Add<TimeInputRenderer>(HtmlRenderer.Time);
        }

        /// <summary>
        /// Adds an IHtmlRenderer to the registration
        /// </summary>
        /// <typeparam name="T">HtmlRender to add to the registration</typeparam>
        /// <param name="name">Dictionary name of the renderer</param>
        /// <exception cref="ArgumentException">Thrown if the name is already registered</exception>
        /// <exception cref="NotSupportedException">Thrown if the generic type is not assignable to IHtmlRenderer</exception>
        /// <exception cref="Exception">Thrown if an instance of the HtmlRenderer cannot be created.</exception>
        public static void Add<T>(string name) {
            if (Renderers.ContainsKey(name.ToLower()))
                throw new ArgumentException($"A renderer with the name {name.ToLower()} has already been registered.", nameof(name));

            Type type = typeof(T);
            if (!typeof(IHtmlRenderer).IsAssignableFrom(type))
                throw new NotSupportedException("The renderer type must be assignable to IHtmlRenderer.");

            object? renderer = Activator.CreateInstance(type);
            if (renderer == null)
                throw new Exception($"Unable to create an instance of {type.FullName}.");

            Renderers.Add(name.ToLower(), (IHtmlRenderer)renderer);
        }

        /// <summary>
        /// Returns the HtmlRender associated with the supplied name
        /// </summary>
        /// <param name="name">Name of the renderer to get</param>
        /// <exception cref="ArgumentException">Thrown if the name is not registered</exception>
        public static IHtmlRenderer Get(string name) {
            CheckDefaultRenderers();
            if (!Renderers.ContainsKey(name.ToLower()))
                throw new ArgumentException($"The key '{name.ToLower()}' was not registered for any HtmlRenderers");

            return Renderers[name.ToLower()];
        }

        /// <summary>
        /// Check if the name is registered in the dictionary
        /// </summary>
        /// <param name="name"></param>
        public static bool Exists(string name) {
            CheckDefaultRenderers();
            return Renderers.ContainsKey(name.ToLower());
        }

        /// <summary>
        /// Returns a HtmlRenderer named 'string'. If it doesn't exist returns the first renderer
        /// </summary>
        public static IHtmlRenderer Default() {
            CheckDefaultRenderers();
            if (Renderers.ContainsKey("string"))
                return Get("string");
            return Renderers.Values.First();
        }

        internal static List<KeyValuePair<string, Type>> GetRendererList() {
            CheckDefaultRenderers();
            return Renderers.Select(x => new KeyValuePair<string, Type>(x.Key, x.Value.GetType())).ToList();
        }

        private static void CheckDefaultRenderers() {
            if (Renderers.Count == 0) {
                RegisterDefaultRenderers();
            }
        }
    }
}
