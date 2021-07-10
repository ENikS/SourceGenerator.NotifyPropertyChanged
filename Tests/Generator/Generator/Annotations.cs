using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Unity.Precompiler.Generators.Tests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;

namespace Unity.Precompiler.Generators.Tests
{
    public partial class PropertyGeneratorTests
    {
        [DataRow("string")]
        [DataRow("private string")]
        [TestMethod, TestProperty(TestingConst, SyntaxReceiver)]
        public async Task WithAttribute(string declaration)
        {
            const string pattern = @"

            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;
            using SourceGenerator.NotifyPropertyChanged;

            namespace ConsoleApplication1
            {
                public class TypeName
                {   
                    [NotifyPropertyChanged]
                    <declaration> field;
                }
            }";
             
            await new VerifyCS
            {
                TestCode = declarationRegex.Replace(pattern, declaration),
            }.RunAsync();
        }
    }
}
