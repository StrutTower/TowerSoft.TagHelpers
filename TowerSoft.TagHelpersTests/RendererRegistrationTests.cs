using System.Reflection;
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
        public void Get_UnregisteredKey_ShouldThrowException() {
            Assert.Throws<ArgumentException>(() => {
                RendererRegistration.Get("unregisteredKey");
            });
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
        public void Add_KeyAlreadyInUse_ShouldThrowException() {
            string rendererKey = "string";
            Assert.Throws<ArgumentException>(() => {
                RendererRegistration.Add<TestRenderer>(rendererKey);
            });
        }

        [TestMethod]
        public void Add_NotIHtmlRenderer_ShouldThrowException() {
            string rendererKey = "notIHtmlRenderer";
            Assert.Throws<NotSupportedException>(() => {
                RendererRegistration.Add<int>(rendererKey);
            });
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

        [TestMethod]
        public void Remove_ShouldRemoveRenderer() {
            string rendererKey = "RemoveTest";
            RendererRegistration.Add<TestRenderer>(rendererKey);

            Assert.IsTrue(RendererRegistration.Exists(rendererKey));

            Assert.IsTrue(RendererRegistration.Remove(rendererKey));

            Assert.IsFalse(RendererRegistration.Exists(rendererKey));
        }

        [TestMethod]
        public void Remove_DoesNotExist_ShouldReturnFalse() {
            Assert.IsFalse(RendererRegistration.Remove("thisdoesnotexist"));
        }

        [TestMethod]
        public void Replace_ShouldReplaceRenderer() {
            string rendererKey = "ReplaceTest";
            RendererRegistration.Add<TestRenderer>(rendererKey);

            RendererRegistration.Replace<TestRenderer2>(rendererKey);

            IHtmlRenderer renderer = RendererRegistration.Get(rendererKey);

            Assert.IsTrue(renderer is TestRenderer2);
        }

        [TestMethod]
        public void Replace_ShouldAddRenderer() {
            string rendererKey = "ReplaceAddTest";

            RendererRegistration.Replace<TestRenderer2>(rendererKey);

            IHtmlRenderer renderer = RendererRegistration.Get(rendererKey);

            Assert.IsTrue(renderer is TestRenderer2);
        }
    }
}
