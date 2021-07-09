using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Unity.Precompiler.Generators.Tests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;

namespace Unity.Precompiler.Generators.Tests
{
    public partial class PropertyGeneratorTests
    {
        /// <summary>
        /// Test declarations with no interface
        /// </summary>
        [DataRow("class TypeName")]
        [DataRow("struct TypeName"               )]
        [DataRow("public class TypeName"         )]
        [DataRow("public struct TypeName"        )]
        [DataRow("public partial class TypeName" )]
        [DataRow("public partial struct TypeName")]
        [TestMethod, TestProperty(TestPropertyName, SyntaxReceiver)]
        public async Task NoInterface(string declaration)
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
            }.RunAsync();
        }


        /// <summary>
        /// Test declarations with irrelevant interfaces
        /// </summary>
        [DataRow("class TypeName : ITestInterface")]
        [DataRow("public class TypeName : ITestInterface")]
        [DataRow("public partial class TypeName : ITestInterface")]
        [DataRow("class TypeName : INotifyPropertyChanged")]
        [DataRow("public class TypeName : INotifyPropertyChanged")]
        [DataRow("public partial class TypeName : INotifyPropertyChanged")]
        [TestMethod, TestProperty(TestPropertyName, SyntaxReceiver)]
        public async Task IncorrectInterface(string declaration)
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
                // Local interface
                public interface ITestInterface { }

                // Local interface overrides INotifyPropertyChanged
                public interface INotifyPropertyChanged { }

                <declaration>
                {   
                }
            }";

            await new VerifyCS
            {
                TestCode = declarationRegex.Replace(pattern, declaration),
            }.RunAsync();
        }

        /// <summary>
        /// Test declarations with interfaces but no partial keyword
        /// </summary>
        [DataRow("class TypeName : {|#0:INotifyPropertyChanged|}")]
        [DataRow("public class TypeName : {|#0:INotifyPropertyChanged|}")]

        [TestMethod, TestProperty(TestPropertyName, SyntaxReceiver)]
        public async Task NoPartialKeyword(string declaration)
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
