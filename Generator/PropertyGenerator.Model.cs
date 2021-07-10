using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace SourceGenerator.NotifyPropertyChanged
{
    public partial class PropertyGenerator
    {
        public sealed class ClassDeclarationModel : List<object>
        {
            private ClassDeclarationSyntax _node;

            public ClassDeclarationModel(ClassDeclarationSyntax node)
            {
                _node = node;
            }

            public Guid Guid = Guid.Empty;

            public bool ImplementsInterface = false;

            public string Namespace = string.Empty;
            public string ClassName = string.Empty;
            public string SourceName = string.Empty;
        }
    }
}
