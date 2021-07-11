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

        public const string AttributeName      = "NotifyPropertyChanged";
        public const string AttributeNameLong  = AttributeName + "Attribute";
        public const string AttributeNamespace = "SourceGenerator.NotifyPropertyChanged";

        #endregion


        #region ISourceGenerator

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var symbol in context.Parse())
            {
                var name = $"{symbol}.Generated.cs";
                var source = symbol.Emit();

                context.AddSource(name, source);
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

            using Stream stream = assembly.GetManifestResourceStream(
                assembly.GetManifestResourceNames().First(n => n.EndsWith("Attribute.cs")));

            using StreamReader reader = new(stream);

            var source = SourceText.From(reader.ReadToEnd(), Encoding.UTF8);

            context.AddSource($"Generated.{AttributeNameLong}.cs", source);
        }

        #endregion
    }
}
