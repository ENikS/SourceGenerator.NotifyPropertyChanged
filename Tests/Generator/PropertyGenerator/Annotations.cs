using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Generator.UnitTests
{
    public partial class PropertyGeneratorTests
    {
        [DataRow("string")]
        [DataRow("private string")]
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task WithAttribute(string declaration)
        {
            // Arrange
            verifyCS.TestCode = declarationRegex.Replace(@" // Test source
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
            }",
            declaration);

            // Verify
            await verifyCS.RunAsync();
        }
    }
}
