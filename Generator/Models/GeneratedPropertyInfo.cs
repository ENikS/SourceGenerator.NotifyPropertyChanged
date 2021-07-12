using Microsoft.CodeAnalysis;

namespace SourceGenerator.NotifyPropertyChanged
{
    public class GeneratedPropertyInfo
    {
        private IFieldSymbol _field;

        public GeneratedPropertyInfo(IFieldSymbol field)
        {
            _field = field;
        }

        public string? Name;

        public bool IsVirtual;

        public bool CacheEvent;

        public string Field => _field.Name;

        public string Type => $"{_field.Type.ContainingNamespace}.{_field.Type.Name}";
    }
}
