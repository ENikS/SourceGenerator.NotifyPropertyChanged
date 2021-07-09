using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceGenerator.NotifyPropertyChanged;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VerifyCS = Unity.Precompiler.Generators.Tests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;

namespace Unity.Precompiler.Generators.Tests
{
    public partial class PropertyGeneratorTests
    {

        /// <summary>
        /// Test declarations with interfaces and keyword
        /// </summary>
        [DataRow("partial class TypeName : INotifyPropertyChanged")]
        [DataRow("public partial class TypeName : INotifyPropertyChanged")]
        [TestMethod, TestProperty(TestPropertyName, PropertyGeneratorName)]
        public async Task WithKeywordAndInterface(string declaration)
        {
            const string pattern = @"

            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;
            using System.ComponentModel;

            namespace ConsoleApplication1
            {
                <declaration>
                {   
                }
            }";

            await new VerifyCS
            {
                TestCode = declarationRegex.Replace(pattern, declaration),
                ExpectedDiagnostics =
                {
                    DiagnosticResult.CompilerError("CS0535")
                                    .WithLocation(0)
                                    .WithArguments("ConsoleApplication1.TypeName", "System.ComponentModel.INotifyPropertyChanged.PropertyChanged")
                },
            }.RunAsync();
        }


    }
}
