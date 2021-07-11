using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using System.Collections.Immutable;

namespace Generator.UnityTests
{
    public class VisualBasicSourceGeneratorTest<TSourceGenerator> : SourceGeneratorTestBase<TSourceGenerator>
            where TSourceGenerator : ISourceGenerator, new()
    {
        public override string Language => LanguageNames.VisualBasic;

        protected override string DefaultFileExt => "vb";

        protected override string DataFolder => nameof(LanguageNames.VisualBasic);


        protected override GeneratorDriver CreateGeneratorDriver(Project project, ImmutableArray<ISourceGenerator> sourceGenerators) 
            => VisualBasicGeneratorDriver.Create(
                sourceGenerators,
                project.AnalyzerOptions.AdditionalFiles,
                (VisualBasicParseOptions)project.ParseOptions!,
                project.AnalyzerOptions.AnalyzerConfigOptionsProvider);

        protected override CompilationOptions CreateCompilationOptions() 
            => new VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        protected override ParseOptions CreateParseOptions() 
            => new VisualBasicParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose);
    }
}
