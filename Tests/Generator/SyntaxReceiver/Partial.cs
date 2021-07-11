using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Receiver.UnitTests
{
    public partial class SyntaxReceiverTests
    {
        [TestMethod, TestProperty(TestingConst, PartialTargetProp)]
        public async Task TwoSources()
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
