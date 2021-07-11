using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Receiver.UnitTests
{
    public partial class SyntaxReceiverTests
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
            verifyCS.TestCode = declarationRegex.Replace(@"// Test source
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                <declaration>
                {   
                }
            }",
            declaration);

            // Verify
            await verifyCS.ExpectGeneratedSource(EmptyJson)
                          .RunAsync();
        }

        [TestMethod, TestProperty(TestingConst, PartialTargetProp)]
        public async Task NothingToGenerateTwoSources()
        {
            // Arrange
            verifyCS.TestState.Sources.Add(@"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                public partial class TypeName
                {   
                    private string field1;
                }
            }");

            verifyCS.TestState.Sources.Add(@"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                public partial class TypeName
                {   
                    private string field2;
                }
            }");

            // Verify
            await verifyCS.ExpectGeneratedSource(EmptyJson)
                          .RunAsync();
        }
    }
}
