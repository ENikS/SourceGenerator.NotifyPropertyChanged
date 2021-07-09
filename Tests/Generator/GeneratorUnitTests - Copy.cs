using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceGenerator.NotifyPropertyChanged;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VerifyCS = Unity.Precompiler.Generators.Tests.CSharpSourceGeneratorTest<SourceGenerator.NotifyPropertyChanged.PropertyGenerator>;

namespace Unity.Precompiler.Generators.Tests
{
    public partial class GeneratorUnitTests
    {


        [TestMethod]
        public async Task NoInterface()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";
                
            await new VerifyCS
            {
                TestState =
                {
                    Sources =
                    {
                        (typeof(PropertyGenerator), "file.cs", test),
                    },
                },
            }.RunAsync();
        }

        [TestMethod]
        public async Task OneInterface()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME : INotifyPropertyChanged
        {   
        }
    }";

            var tst = new VerifyCS
            {
                TestState =
                {
                    Sources =
                    {
                        (typeof(PropertyGenerator), "file.cs", test),
                    },
                    GeneratedSources =
                    {
                        (typeof(PropertyGenerator), PropertyGenerator.ResourceName, GeneratedAttributeSource),
                    }
                },
            };

            try
            {
                await tst.RunAsync();
            }
            catch (System.Exception ex)
            {
                throw;
            }

            var dd = tst.TestState.GeneratedSources;
        }

    }
}
