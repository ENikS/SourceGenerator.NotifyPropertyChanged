using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SourceGenerator.NotifyPropertyChanged
{
    public static class Parser
    {
        public static IEnumerable<INamedTypeSymbol> Parse(this GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not Receiver receiver || 0 == receiver.Count)
                yield break;

            // No Attribute
            INamedTypeSymbol? autogenerateAttribute = context.Compilation.GetTypeByMetadataName(typeof(NotifyPropertyChangedAttribute).FullName);
            if (autogenerateAttribute is null)
                yield break;

            var set = new HashSet<string>();

            // we enumerate by syntax tree, to minimize the need to instantiate semantic models (since they're expensive)
            foreach (IGrouping<SyntaxTree, ClassDeclarationSyntax>? group in receiver.GroupBy(x => x.SyntaxTree))
            {
                SemanticModel? sm = null;
                foreach (ClassDeclarationSyntax? classDef in group)
                {
                    // Stop if generation is canceled
                    if (context.CancellationToken.IsCancellationRequested)
                        yield break;

                    // Get symbol for the declaration
                    sm ??= context.Compilation.GetSemanticModel(classDef.SyntaxTree);
                    if (sm.GetDeclaredSymbol(classDef) is not INamedTypeSymbol symbol)
                        continue;

                    // Get interface
                    var @interface = symbol.Interfaces.First(i => nameof(INotifyPropertyChanged).Equals(i.Name));
                    if (!typeof(INotifyPropertyChanged).Namespace.Equals(@interface.ContainingNamespace.ToString())) 
                        continue;

                    // Only distinct symbols
                    if (!set.Add(symbol.ToString())) continue;
                        
                    yield return symbol;
                }
            }
        }
    }
}
