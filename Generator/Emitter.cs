using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGenerator.NotifyPropertyChanged
{
    public static class Emitter
    {
        public static SourceText Emit(this INamedTypeSymbol symbol)
        {
            var sb = new StringBuilder();


            return SourceText.From(sb.ToString(), Encoding.UTF8);
        }
    }
}
