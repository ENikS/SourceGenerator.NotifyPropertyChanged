﻿using Microsoft.CodeAnalysis;

namespace SourceGenerator.NotifyPropertyChanged
{
    [Generator]
    public partial class PropertyGenerator : ISourceGenerator
    {


        #region ISourceGenerator

        public void Execute(GeneratorExecutionContext context)
        {
            SyntaxReceiver? receiver = context.SyntaxContextReceiver as SyntaxReceiver;
            if ((receiver?.Count ?? 0) == 0) return;

            //Parser? p = new Parser(context.Compilation, context.ReportDiagnostic, context.CancellationToken);
            //EventSourceClass[]? eventSources = p.GetEventSourceClasses(receiver.CandidateClasses);

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