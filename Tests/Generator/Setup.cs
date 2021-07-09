using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceGenerator.NotifyPropertyChanged;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VerifyCS = Unity.Precompiler.Generators.Tests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;


namespace Unity.Precompiler.Generators.Tests
{
    [TestClass]
    public partial class PropertyGeneratorTests
    {
        #region Constants

        protected const string TestPropertyName  = "Testing";
        protected const string PropertyGeneratorName = nameof(PropertyGenerator);
        protected const string SyntaxReceiver = nameof(PropertyGenerator.SyntaxReceiver);

        #endregion


        #region Fields

        protected static SourceText GeneratedAttributeSource;
        protected static readonly Regex declarationRegex = new("<declaration>", RegexOptions.Compiled);

        #endregion


        #region Constructors

        public PropertyGeneratorTests()
        {
            using Stream stream = typeof(PropertyGenerator)
                .Assembly.GetManifestResourceStream(PropertyGenerator.ResourceName);
            using StreamReader reader = new(stream);

            GeneratedAttributeSource = SourceText.From(reader.ReadToEnd(), Encoding.UTF8);
        }

        #endregion


        #region Tests

        [TestMethod, TestProperty(TestPropertyName, nameof(CSharpSourceGeneratorTest<PropertyGenerator>))]
        public async Task Baseline() => await new VerifyCS().RunAsync();

        #endregion
    }
}
