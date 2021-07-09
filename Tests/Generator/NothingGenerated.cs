using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceGenerator.NotifyPropertyChanged;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VerifyCS = Unity.Precompiler.Generators.Tests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;

namespace Unity.Precompiler.Generators.Tests
{
    public partial class GeneratorUnitTests
    {
        const string pattern = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

        public static IEnumerable<object[]> Parameters_Test_Data
        {
            get
            {
                yield return new object[] 
                {
                    "class TYPENAME",
                };
            }
        }


        [TestMethod]
        [DynamicData(nameof(Parameters_Test_Data))]
        public async Task NothingToGenerate(string test)
        {
            const string pattern = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

            await new VerifyCS
            {
                TestState =
                {
                    Sources =
                    {
                        (typeof(PropertyGenerator), "TestSource.cs", source),
                    },
                    GeneratedSources =
                    {
                        (typeof(PropertyGenerator), PropertyGenerator.ResourceName, GeneratedAttributeSource),
                    }
                },
            }.RunAsync();
        }


        [TestMethod]
        [DynamicData(nameof(Parameters_Test_Data))]
        public async Task NothingToGenerate1(string declaration)
        {
            var result = new DiagnosticResult().WithArguments(declaration); 
            var tst = new VerifyCS
            {
                TestCode = pattern,
            };

//            tst.TestState.GeneratedSources.Add((typeof(PropertyGenerator), PropertyGenerator.ResourceName, GeneratedAttributeSource));
            tst.TestState.ExpectedDiagnostics.Add(result);

            try
            {
                await tst.RunAsync();
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }
    }
}
