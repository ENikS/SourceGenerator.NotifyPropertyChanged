using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SourceGenerator.NotifyPropertyChanged
{
    public partial class PropertyGenerator
    {
        private sealed class SyntaxReceiver : List<ClassDeclarationSyntax>, ISyntaxReceiver
        {
            #region Constants

            private const string _name = nameof(System.ComponentModel.INotifyPropertyChanged);

            #endregion


            #region Visitor

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode.RawKind != (int)SyntaxKind.SimpleBaseType) return;

                var node = (SimpleBaseTypeSyntax)syntaxNode;

                // Check if Span length matches before allocating the string to check more
                if (node.Span.Length != _name.Length) return;

                // Possible match, now check the string value
                if (node.ToString() != _name) return;

                // Match add to candidates
                Add((ClassDeclarationSyntax)node.Parent!.Parent!);
            }

            #endregion
        }
    }
}
