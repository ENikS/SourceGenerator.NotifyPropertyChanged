using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Generator.UnitTests
{
    public partial class PropertyGeneratorTests
    {
        /// <summary>
        /// Test declarations with no interface
        /// </summary>
        [DataRow("class TypeName")]
        [DataRow("struct TypeName")]
        [DataRow("public class TypeName")]
        [DataRow("public struct TypeName")]
        [DataRow("public partial class TypeName")]
        [DataRow("public partial struct TypeName")]
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task NoInterface(string declaration)
        {
            // Arrange
            verifyCS.TestCode = declarationRegex.Replace(@" // Test source
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
            }",
            declaration);

            // Verify
            await verifyCS.RunAsync();
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
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task IncorrectInterface(string declaration)
        {
            // Arrange
            verifyCS.TestCode = declarationRegex.Replace(@" // Test source
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
            }",
            declaration);

            // Verify
            await verifyCS.RunAsync();
        }

        /// <summary>
        /// Test declarations with interfaces but no partial keyword
        /// </summary>
        [DataRow("class TypeName : {|#0:INotifyPropertyChanged|}")]
        [DataRow("public class TypeName : {|#0:INotifyPropertyChanged|}")]

        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task NoPartialKeyword(string declaration)
        {
            // Arrange
            verifyCS.TestCode = declarationRegex.Replace(@" // Test source
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
            }", declaration);

            // Diagnostics
            verifyCS.ExpectedDiagnostics.Add(
                DiagnosticResult.CompilerError("CS0535")
                                .WithLocation(0)
                                .WithArguments("ConsoleApplication1.TypeName", "System.ComponentModel.INotifyPropertyChanged.PropertyChanged"));

            // Verify
            await verifyCS.RunAsync();
        }
    }
}
