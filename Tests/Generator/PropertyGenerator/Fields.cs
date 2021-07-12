using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generator.UnitTests
{
    public partial class PropertyGeneratorTests
    {
        public static IEnumerable<object[]> Property_Test_Data
        {
            get
            {
                yield return new object[] { "private int _property" };
                yield return new object[] { "private string _property" };
            }
        }


        /// <summary>
        /// Test declarations with interfaces and keyword
        /// </summary>
        [DynamicData(nameof(Property_Test_Data))]
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task WithNakedField(string declaration)
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
                partial class TypeName : INotifyPropertyChanged
                {   
                    <declaration>
                }
            }", 
            declaration);
            
            // Verify
            await verifyCS.RunAsync();
        }

        /// <summary>
        /// Test declarations with interfaces and keyword
        /// </summary>
        [DynamicData(nameof(Property_Test_Data))]
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task WithAnnotatedField(string declaration)
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
                partial class TypeName : INotifyPropertyChanged
                {   
                    [SourceGenerator.NotifyPropertyChanged.NotifyPropertyChanged]
                    <declaration>
                }
            }",
            declaration);

            // Verify
            await verifyCS.RunAsync();
        }

        /// <summary>
        /// Test declarations with interfaces and keyword
        /// </summary>
        [DynamicData(nameof(Property_Test_Data))]
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task AnnotatedFieldWithName(string declaration)
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
                partial class TypeName : INotifyPropertyChanged
                {   
                    [SourceGenerator.NotifyPropertyChanged.NotifyPropertyChanged(""Name"")]
                    <declaration>
                }
            }",
            declaration);

            // Verify
            await verifyCS.RunAsync();
        }


        /// <summary>
        /// Test declarations with interfaces and keyword
        /// </summary>
        [DynamicData(nameof(Property_Test_Data))]
        [TestMethod, TestProperty(TestingConst, SingleTargetProp)]
        public async Task FieldWithNameVirtual(string declaration)
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
                partial class TypeName : INotifyPropertyChanged
                {   
                    [SourceGenerator.NotifyPropertyChanged.NotifyPropertyChanged(""Name"", IsVirtual = true)]
                    <declaration>
                }
            }",
            declaration);

            // Verify
            await verifyCS.RunAsync();
        }
    }
}
