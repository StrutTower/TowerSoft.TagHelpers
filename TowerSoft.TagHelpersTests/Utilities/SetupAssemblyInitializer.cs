using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
