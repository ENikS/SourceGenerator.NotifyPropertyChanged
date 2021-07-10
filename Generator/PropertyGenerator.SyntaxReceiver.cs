using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SourceGenerator.NotifyPropertyChanged
{
    public partial class PropertyGenerator
    {
        public sealed class SyntaxReceiver : Dictionary<ClassDeclarationSyntax, ClassDeclarationModel>, ISyntaxContextReceiver
        {
            #region Visitor

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                switch (context.Node.RawKind)
                {
                    case (int)SyntaxKind.Attribute:
                        OnAttribute(context);
                        break;

                    case (int)SyntaxKind.SimpleBaseType:
                        OnSimpleBaseType(context);
                        break;
                }
            }

            private void OnAttribute(GeneratorSyntaxContext context)
            {
                // Filter out unrelated syntax
                if (context.Node is not AttributeSyntax node ||
                    (node.Span.Length != AttributeName.Length && node.Span.Length != AttributeNameLong.Length) ||
                    !node.ToString().StartsWith(AttributeName) ||
                    node.Parent is null || node.Parent.Parent is null || node.Parent.Parent.Parent is null)
                    return;

                var field = (FieldDeclarationSyntax)node.Parent.Parent;
                var declaration = (ClassDeclarationSyntax)node.Parent.Parent.Parent;

                if (!TryGetValue(declaration, out var model))
                {
                    model = new ClassDeclarationModel(declaration);
                    Add(declaration, model);
                }

                model.Add(field);
            }

            #endregion


            #region Implementation

            private void OnSimpleBaseType(GeneratorSyntaxContext context)
            {
                // Filter out unrelated syntax
                if (context.Node is not SimpleBaseTypeSyntax node ||
                    node.Span.Length != InterfaceName.Length ||
                    node.ToString() != InterfaceName ||
                    node.Parent is null || node.Parent.Parent is null)
                    return;

                var declaration = (ClassDeclarationSyntax)node.Parent.Parent;

                // Is partial keyword present
                if (!declaration.Modifiers.Any(m => SyntaxKind.PartialKeyword == m.Kind())) return;

                // Interface matches
                var symbol = context.SemanticModel.GetDeclaredSymbol(declaration);
                if (symbol is null) return;
                var @interface = symbol.Interfaces.First(i => InterfaceName.Equals(i.Name));
                if (!InterfaceNamespace.Equals(@interface.ContainingNamespace.ToString())) return;

                // Full match, add to the list
                Add(declaration, new ClassDeclarationModel(declaration));
            }

            #endregion
        }
    }
}
