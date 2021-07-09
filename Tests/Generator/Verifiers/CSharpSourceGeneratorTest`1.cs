using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGenerator.NotifyPropertyChanged;
using System.Collections.Immutable;

namespace Unity.Precompiler.Generators.Tests
{
    public class CSharpSourceGeneratorTest<TSourceGenerator> : SourceGeneratorTestBase<TSourceGenerator>
        where TSourceGenerator : ISourceGenerator, new()
    {
        public CSharpSourceGeneratorTest()
        {
            TestState.GeneratedSources.Add((typeof(PropertyGenerator), PropertyGenerator.ResourceName, GeneratedAttributeSource));
        }

        public override string Language => LanguageNames.CSharp;

        protected override string DefaultFileExt => "cs";

        protected override string DataFolder => nameof(LanguageNames.CSharp);


        protected override GeneratorDriver CreateGeneratorDriver(Project project, ImmutableArray<ISourceGenerator> sourceGenerators) 
            => CSharpGeneratorDriver.Create(
                sourceGenerators,
                project.AnalyzerOptions.AdditionalFiles,
                (CSharpParseOptions)project.ParseOptions!,
                project.AnalyzerOptions.AnalyzerConfigOptionsProvider);

        protected override CompilationOptions CreateCompilationOptions() 
            => new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        protected override ParseOptions CreateParseOptions() 
            => new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose);
    }
}
