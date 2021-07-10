using Microsoft.CodeAnalysis;

namespace SourceGenerator.NotifyPropertyChanged
{
    [Generator]
    public partial class PropertyGenerator : ISourceGenerator
    {
        #region Constants

        public const string ResourceName = "SourceGenerator.NotifyPropertyChanged.Templates.NotifyPropertyChangedAttribute.cs";

        public const string AttributeName      = "NotifyPropertyChanged";
        public const string AttributeNameLong  = AttributeName + "Attribute";
        public const string AttributeNamespace = "SourceGenerator.NotifyPropertyChanged";

        public const string InterfaceName      = "INotifyPropertyChanged";
        public const string InterfaceFullName  = "System.ComponentModel.INotifyPropertyChanged";
        public const string InterfaceNamespace = "System.ComponentModel";

        #endregion


        #region ISourceGenerator

        public void Execute(GeneratorExecutionContext context)
        {
            SyntaxReceiver? receiver = context.SyntaxContextReceiver as SyntaxReceiver;
            if ((receiver?.Count ?? 0) == 0) return;

            Parser p = new Parser(context.Compilation, context.ReportDiagnostic, context.CancellationToken);
            //ClassDeclarationModel[]? eventSources = p.GetEventSourceClasses(receiver);

            //if (eventSources?.Length > 0)
            //{
            //    Emitter? e = new Emitter(context);
            //    e.Emit(eventSources, context.CancellationToken);
            //}
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
            context.RegisterForPostInitialization(OnPostInitialize);
        }

        #endregion
    }
}
