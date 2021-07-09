using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Unity.Precompiler.Generators.Tests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;


namespace Unity.Precompiler.Generators.Tests
{
    [TestClass]
    public partial class GeneratorUnitTests : GeneratorTestBase
    {
        [TestMethod]
        public async Task BaselineTest()
        {
            await new VerifyCS().RunAsync();
        }
    }
}
