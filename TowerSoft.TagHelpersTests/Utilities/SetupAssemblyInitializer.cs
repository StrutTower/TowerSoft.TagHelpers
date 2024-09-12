using TowerSoft.TagHelpers.Options;

namespace TowerSoft.TagHelpersTests.Utilities {
    [TestClass]
    public class SetupAssemblyInitializer {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context) {
            RendererRegistration.RegisterDefaultRenderers();
        }
    }
}
