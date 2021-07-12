using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceGenerator.NotifyPropertyChanged
{
    [Generator]
    public partial class PropertyGenerator : ISourceGenerator
    {
        #region Constants

        public const string AttributeName = "NotifyPropertyChanged";
        public const string AttributeNameLong = AttributeName + "Attribute";
        public const string AttributeNamespace = "SourceGenerator.NotifyPropertyChanged";

        #endregion


        #region ISourceGenerator

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var symbol in context.Parse())
            {
                var source = symbol.Emit();
                if (source is null) continue;

                context.AddSource($"{symbol.Namespace}.{symbol.Name}.cs",
                                  SourceText.From(source, Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new Receiver());
            context.RegisterForPostInitialization(OnPostInitialize);
        }

        #endregion


        #region Initialization

        /// <summary>
        /// Inject implementation of a marker attribute into target codebase
        /// </summary>
        private void OnPostInitialize(GeneratorPostInitializationContext context)
        {
            var assembly = GetType().Assembly;
            var name = assembly.GetManifestResourceNames().First(n => n.EndsWith("Attribute.cs"));

            using Stream stream = assembly.GetManifestResourceStream(name);
            using StreamReader reader = new(stream);

            var source = SourceText.From(reader.ReadToEnd(), Encoding.UTF8);
            context.AddSource($"Generated.{AttributeNameLong}.cs", source);
        }

        #endregion
    }
}
