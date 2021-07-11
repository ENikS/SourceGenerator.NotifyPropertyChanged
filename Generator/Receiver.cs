using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SourceGenerator.NotifyPropertyChanged
{
    public sealed class Receiver : HashSet<ClassDeclarationSyntax>, ISyntaxReceiver
    {
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            switch (syntaxNode.RawKind)
            {
                case (int)SyntaxKind.Attribute:
                    OnAttribute(syntaxNode);
                    break;

                case (int)SyntaxKind.SimpleBaseType:
                    OnSimpleBaseType(syntaxNode);
                    break;
            }
        }


        private void OnAttribute(SyntaxNode syntaxNode)
        {
            // Filter out unrelated syntax
            if (syntaxNode is not AttributeSyntax node ||
                (node.Span.Length != PropertyGenerator.AttributeName.Length && node.Span.Length != PropertyGenerator.AttributeNameLong.Length) ||
                !node.ToString().StartsWith(PropertyGenerator.AttributeName))
                return;

            var declaration = (ClassDeclarationSyntax?)node.Parent?.Parent?.Parent;

            if (declaration is not null && declaration.Modifiers.Any(m => SyntaxKind.PartialKeyword == m.Kind()))
            { 
                // Add if partial keyword is present
                Add(declaration);
            }
        }

        private void OnSimpleBaseType(SyntaxNode syntaxNode)
        {
            // Filter out unrelated syntax
            if (syntaxNode is not SimpleBaseTypeSyntax node ||
                node.Span.Length != nameof(INotifyPropertyChanged).Length ||
                node.ToString() != nameof(INotifyPropertyChanged))
                return;

            var declaration = (ClassDeclarationSyntax?)node.Parent?.Parent;

            if (declaration is not null && declaration.Modifiers.Any(m => SyntaxKind.PartialKeyword == m.Kind()))
            { 
                // Add if partial keyword is present
                Add(declaration);
            }
        }
    }
}
