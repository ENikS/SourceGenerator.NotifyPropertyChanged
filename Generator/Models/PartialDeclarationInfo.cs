using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SourceGenerator.NotifyPropertyChanged
{

    public class PartialDeclarationInfo : List<GeneratedPropertyInfo>
    {
        #region Filds

        private readonly ClassDeclarationSyntax _syntax;
        private INamedTypeSymbol _symbol;
        public readonly bool HasEvent;

        #endregion


        #region Constructors

        public PartialDeclarationInfo(ClassDeclarationSyntax syntax, INamedTypeSymbol symbol, bool hasEvent, IEnumerable<GeneratedPropertyInfo> attributedFields)
            : base(attributedFields)
        {
            _syntax = syntax;
            _symbol = symbol;
            HasEvent = hasEvent;
        }

        #endregion


        #region Properties
        
        public string Name => _symbol.Name;

        public string Namespace => _symbol.ContainingNamespace.Name;

        public string Modifiers => _syntax.Modifiers.ToString();

        public string Declaration => _symbol.DeclaredAccessibility.ToString();

        #endregion
    }
}
