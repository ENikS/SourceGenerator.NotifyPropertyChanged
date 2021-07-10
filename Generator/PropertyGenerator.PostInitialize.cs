using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Reflection;
using System.Text;

namespace SourceGenerator.NotifyPropertyChanged
{
    public partial class PropertyGenerator
    {
        /// <summary>
        /// Inject implementation of a marker attribute into target codebase
        /// </summary>
        private void OnPostInitialize(GeneratorPostInitializationContext context)
        {
            using Stream stream = Assembly.GetExecutingAssembly()
                                          .GetManifestResourceStream(ResourceName);
            
            using StreamReader reader = new(stream);

            var source = SourceText.From(reader.ReadToEnd(), Encoding.UTF8);
            context.AddSource(ResourceName, source);
        }
    }
}
