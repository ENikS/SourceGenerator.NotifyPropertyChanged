using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generator.UnitTests
{
    public partial class PropertyGeneratorTests
    {
        public static IEnumerable<object[]> DeclarationWithInterface_Test_Data
        {
            get
            {
                yield return new object[] { "partial class TypeName : INotifyPropertyChanged"                              };
                yield return new object[] { "partial class TypeName : System.ComponentModel.INotifyPropertyChanged"        };
                yield return new object[] { "public partial class TypeName : INotifyPropertyChanged"                       };
                yield return new object[] { "public partial class TypeName : System.ComponentModel.INotifyPropertyChanged" };
            }
        }


        /// <summary>
        /// Test declarations with interfaces and keyword
        /// </summary>
        [DynamicData(nameof(DeclarationWithInterface_Test_Data))]
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task WithKeywordAndInterface(string declaration)
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
        /// Test declarations with interfaces and keyword
        /// </summary>
        [DynamicData(nameof(DeclarationWithInterface_Test_Data))]
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task WithImplementedInterface(string declaration)
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
                    public event PropertyChangedEventHandler PropertyChanged;
                }
            }", 
            declaration);

            // Verify
            await verifyCS.RunAsync();
        }

    }
}
