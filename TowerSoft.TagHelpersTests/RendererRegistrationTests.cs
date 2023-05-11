using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TowerSoft.TagHelpers.HtmlRenderers;
using TowerSoft.TagHelpers.Interfaces;
using TowerSoft.TagHelpers.Options;
using TowerSoft.TagHelpersTests.HtmlRenderers;

namespace TowerSoft.TagHelpersTests {
    [TestClass]
    public class RendererRegistrationTests {
        [TestMethod]
        public void RendererRegistrationDefaults_ShouldRegisterAllHtmlRenderers() {
            Assembly assembly = Assembly.GetAssembly(typeof(RendererRegistration));
            List<Type> renderers = assembly.GetTypes().Where(x => x.IsAssignableTo(typeof(IHtmlRenderer))).ToList();

            List<KeyValuePair<string, Type>> registeredRenderers = RendererRegistration.GetRendererList();

            foreach (Type renderer in renderers.Where(x => !x.IsInterface)) {
                Assert.IsTrue(registeredRenderers.Select(x => x.Value).Contains(renderer));
            }
        }

        [TestMethod]
        public void Get_ShouldReturnStringRenderer() {
            IHtmlRenderer htmlRenderer = RendererRegistration.Get("string");
            Assert.IsNotNull(htmlRenderer);
            Assert.IsInstanceOfType(htmlRenderer, typeof(StringInputRenderer));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_UnregisteredKey_ShouldThrowException() {
            RendererRegistration.Get("unregisteredKey");
        }

        [TestMethod]
        public void Add_ShouldAddNewRenderer() {
            string rendererKey = "test";
            RendererRegistration.Add<TestRenderer>(rendererKey);

            IHtmlRenderer htmlRenderer = RendererRegistration.Get(rendererKey);

            Assert.IsNotNull(htmlRenderer);
            Assert.AreEqual(typeof(TestRenderer), htmlRenderer.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_KeyAlreadyInUse_ShouldThrowException() {
            string rendererKey = "string";
            RendererRegistration.Add<TestRenderer>(rendererKey);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Add_NotIHtmlRenderer_ShouldThrowException() {
            string rendererKey = "notIHtmlRenderer";
            RendererRegistration.Add<int>(rendererKey);
        }

        [TestMethod]
        public void Exists_RegisteredKey_ShouldReturnTrue() {
            Assert.IsTrue(RendererRegistration.Exists("string"));
        }

        [TestMethod]
        public void Exists_UnregisteredKey_ShouldReturnFalse() {
            Assert.IsFalse(RendererRegistration.Exists("unregisteredKey"));
        }

        [TestMethod]
        public void Default_ShouldReturnStringRenderer() {
            IHtmlRenderer htmlRenderer = RendererRegistration.Default();
            Assert.IsInstanceOfType(htmlRenderer, typeof(StringInputRenderer));
        }
    }
}
