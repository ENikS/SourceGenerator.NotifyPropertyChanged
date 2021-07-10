using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyCS = Unity.Precompiler.Generators.Tests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;

namespace Unity.Precompiler.Generators.Tests
{
    public partial class PropertyGeneratorTests
    {
        public static IEnumerable<object[]> Declarations_Test_Data
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
        [DynamicData(nameof(Declarations_Test_Data))]
        [TestMethod, TestProperty(TestingConst, PropertyGeneratorName)]
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
            }.RunAsync();
        }

        /// <summary>
        /// Test declarations with interfaces and keyword
        /// </summary>
        [DynamicData(nameof(Declarations_Test_Data))]
        [TestMethod, TestProperty(TestingConst, PropertyGeneratorName)]
        public async Task WithImplementedInterface(string declaration)
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
                    public event PropertyChangedEventHandler PropertyChanged;
                }
            }";

            await new VerifyCS
            {
                TestCode = declarationRegex.Replace(pattern, declaration),
            }.RunAsync();
        }

    }
}
