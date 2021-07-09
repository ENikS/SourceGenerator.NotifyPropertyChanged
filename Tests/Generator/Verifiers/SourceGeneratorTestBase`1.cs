using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using SourceGenerator.NotifyPropertyChanged;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Unity.Precompiler.Generators.Tests
{
    public abstract class SourceGeneratorTestBase<TSourceGenerator> : SourceGeneratorTest<DefaultVerifier>
        where TSourceGenerator : ISourceGenerator, new()
    {
        #region Fields

        protected static SourceText GeneratedAttributeSource;

        #endregion


        public SourceGeneratorTestBase()
        {
            using Stream stream = typeof(PropertyGenerator).Assembly
                                                           .GetManifestResourceStream(PropertyGenerator.ResourceName);
            using StreamReader reader = new(stream);

            GeneratedAttributeSource = SourceText.From(reader.ReadToEnd(), Encoding.UTF8);
        }


        protected abstract string DataFolder { get; }

        protected override IEnumerable<ISourceGenerator> GetSourceGenerators()
        {
            yield return new TSourceGenerator();
        }
    }
}
