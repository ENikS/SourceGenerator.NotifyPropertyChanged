using Microsoft.CodeAnalysis.Text;
using SourceGenerator.NotifyPropertyChanged;
using System.IO;
using System.Text;

namespace Unity.Precompiler.Generators.Tests
{
    public class GeneratorTestBase
    {
        #region Fields

        protected static SourceText GeneratedAttributeSource;

        #endregion


        public GeneratorTestBase()
        {
            using Stream stream = typeof(PropertyGenerator).Assembly
                                                           .GetManifestResourceStream(PropertyGenerator.ResourceName);
            using StreamReader reader = new(stream);

            GeneratedAttributeSource = SourceText.From(reader.ReadToEnd(), Encoding.UTF8);
        }
    }
}
