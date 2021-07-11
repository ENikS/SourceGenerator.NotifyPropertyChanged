using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceGenerator.NotifyPropertyChanged;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VerifyCS = Generator.UnityTests
    .CSharpSourceGeneratorTest<Receiver.UnitTests
        .SyntaxReceiverTests.ReceiverSpyGenerator<SourceGenerator.NotifyPropertyChanged.Receiver>>;

namespace Receiver.UnitTests
{
    [TestClass]
    public partial class SyntaxReceiverTests
    {
        #region Constants

        protected const string TestingConst  = "Testing";
        protected const string SingleTargetProp = "Single Source";
        protected const string PartialTargetProp = "Multi Source";

        protected const string EmptyJson = @"/*[]*/";


        protected const string GeneratedPath = "file.cs";
        protected static Type GeneratorType = typeof(ReceiverSpyGenerator<PropertyGenerator.SyntaxReceiver>);

        #endregion


        #region Fields

        protected VerifyCS verifyCS;
        protected static readonly Regex declarationRegex = new("<declaration>", RegexOptions.Compiled);

        #endregion


        #region Initialization

        [TestInitialize]
        public void InitializeTest()
        {
            verifyCS = new VerifyCS();
        }

        #endregion


        #region Internal Types

        public sealed class ReceiverSpyGenerator<TReceiver> : ISourceGenerator
            where TReceiver : ISyntaxContextReceiver, new()
        {
            public void Execute(GeneratorExecutionContext context)
            {
                var receiver = (TReceiver)context.SyntaxContextReceiver;
                var json = JsonSerializer.Serialize(receiver, typeof(TReceiver));
                var sb = new StringBuilder();

                sb.Append(@"/*");
                sb.Append(json);
                sb.Append(@"*/");

                context.AddSource(GeneratedPath, sb.ToString());
            }

            public void Initialize(GeneratorInitializationContext context)
                => context.RegisterForSyntaxNotifications(() => new TReceiver());
        }

        #endregion


        #region Tests

        [TestMethod, TestProperty(TestingConst, "SyntaxReceiver")]
        public async Task Baseline() 
            => await verifyCS.ExpectGeneratedSource(EmptyJson)
                             .RunAsync();

        #endregion
    }
}
