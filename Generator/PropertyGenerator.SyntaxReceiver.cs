using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SourceGenerator.NotifyPropertyChanged
{
    public partial class PropertyGenerator
    {
        public sealed class SyntaxReceiver : List<ClassDeclarationSyntax>, ISyntaxContextReceiver
        {
            #region Constants

            private const  string _name = nameof(INotifyPropertyChanged);
            private static string _namespace = typeof(INotifyPropertyChanged).Namespace;

            #endregion


            #region Visitor

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                // Filter out unrelated syntax
                if (context.Node.RawKind != (int)SyntaxKind.SimpleBaseType ||
                    context.Node is not SimpleBaseTypeSyntax node ||
                    node.Span.Length != _name.Length ||
                    node.ToString()  != _name ||
                    node.Parent is null || node.Parent.Parent is null) 
                    return;

                var parent = (ClassDeclarationSyntax)node.Parent.Parent;

                // Is partial keyword present
                if (!parent.Modifiers.Any(m => SyntaxKind.PartialKeyword == m.Kind())) return;
                
                // Interface matches
                var symbol = context.SemanticModel.GetDeclaredSymbol(parent);
                if (symbol is null) return;
                var @interface = symbol.Interfaces.First(i => _name.Equals(i.Name));
                if (!_namespace.Equals(@interface.ContainingNamespace.ToString())) return;

                // Full match, add to the list
                Add(parent);
            }

            #endregion
        }
    }
}
