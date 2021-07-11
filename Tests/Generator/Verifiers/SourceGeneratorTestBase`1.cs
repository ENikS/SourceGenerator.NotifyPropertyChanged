using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Text;

namespace Generator.UnityTests
{
    public abstract class SourceGeneratorTestBase<TSourceGenerator> : SourceGeneratorTest<DefaultVerifier>
        where TSourceGenerator : ISourceGenerator, new()
    {
        protected abstract string DataFolder { get; }

        protected override IEnumerable<ISourceGenerator> GetSourceGenerators()
        {
            yield return new TSourceGenerator();
        }

        public SourceGeneratorTestBase<TSourceGenerator> ExpectGeneratedSource(string content, string path = "file.cs")
        {
            TestState.GeneratedSources.Add((typeof(TSourceGenerator), path,
                           SourceText.From(content, Encoding.UTF8)));

            return this;
        }
    }
}
