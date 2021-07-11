using Generator.UnityTests;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceGenerator.NotifyPropertyChanged;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VerifyCS = Generator.UnityTests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;


namespace Generator.UnitTests
{
    [TestClass]
    public partial class PropertyGeneratorTests
    {
        #region Constants

        protected const string TestingConst  = "Testing";
        protected const string SingleTargetProp = "Single Source";
        protected const string PartialTargetProp = "Multi Source";

        #endregion


        #region Fields

        protected static SourceText GeneratedAttributeSource;
        protected static readonly Regex declarationRegex = new("<declaration>", RegexOptions.Compiled);

        protected VerifyCS verifyCS;

        #endregion

        
        #region Constructors

        public PropertyGeneratorTests()
        {
            var assembly = typeof(PropertyGenerator).Assembly;

            using Stream stream = assembly.GetManifestResourceStream(
                assembly.GetManifestResourceNames().First(n => n.EndsWith("Attribute.cs")));

            using StreamReader reader = new(stream);

            GeneratedAttributeSource = SourceText.From(reader.ReadToEnd(), Encoding.UTF8);
        }

        #endregion


        #region Initialization

        [TestInitialize]
        public void InitializeTest()
        {
            verifyCS = new VerifyCS();
            verifyCS.TestState.GeneratedSources
                    .Add((typeof(PropertyGenerator), "Generated.NotifyPropertyChangedAttribute.cs", GeneratedAttributeSource));
        }

        #endregion


        #region Tests

        [TestMethod, TestProperty(TestingConst, nameof(CSharpSourceGeneratorTest<PropertyGenerator>))]
        public async Task Baseline()
        {
            await verifyCS.RunAsync();
        }

        #endregion
    }
}
