using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

namespace SourceGenerator.NotifyPropertyChanged
{
    public static class Parser
    {
        public static IEnumerable<PartialDeclarationInfo> Parse(this GeneratorExecutionContext context)
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

                    yield return new PartialDeclarationInfo(classDef, symbol,
                        symbol.MemberNames.Any(member => nameof(INotifyPropertyChanged.PropertyChanged) == member),
                        symbol.GetMembers().Where(s => SymbolKind.Field == s.Kind).Cast<IFieldSymbol>().Parse());
                }
            }
        }

        private static IEnumerable<GeneratedPropertyInfo> Parse(this IEnumerable<IFieldSymbol> fields)
        {
            foreach (IFieldSymbol field in fields)
            {
                var attribute = field.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == nameof(NotifyPropertyChangedAttribute));
                if (attribute is null) continue;

                yield return field.Parse(attribute);
            }
        }


        public static GeneratedPropertyInfo Parse(this IFieldSymbol field, AttributeData attribute)
        {
            var info = new GeneratedPropertyInfo(field)
            { 
                Name = field.Parse(attribute.ConstructorArguments)
            };

            foreach (var argument in attribute.NamedArguments)
            {
                switch (argument.Key)
                {
                    case nameof(NotifyPropertyChangedAttribute.IsVirtual):
                        info.IsVirtual = (bool)(argument.Value.Value ?? false);
                        break;

                    case nameof(NotifyPropertyChangedAttribute.CacheEvent):
                        info.CacheEvent = (bool)(argument.Value.Value ?? false);
                        break;
                }
            }

            return info;
        }


        private static string Parse(this IFieldSymbol field, ImmutableArray<TypedConstant> arguments)
        {
            if (0 != arguments.Length)
            { 
                return arguments[0].Value?.ToString() ?? "InvalidName";
            }

            return field.Name;
        }
    }
}
