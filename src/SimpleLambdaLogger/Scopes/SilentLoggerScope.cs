using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
namespace SimpleLambdaLogger
{
    internal class SilentLoggerScope : BaseLoggerScope
    {
        public SilentLoggerScope(BaseLoggerScope parentScope)
        {
            ParentScope = parentScope;
        }

        public override void Dispose()
        {
            if (ParentScope != null)
            {
                SimpleLogger.CurrentScope.Value = ParentScope;
                return;
            }
            
            SimpleLogger.CurrentScope.Value = null; // memory leaks without this line.
        }

        public override void Log(LogEventLevel logEventLevel, string message, params object[] args)
        {
        }

        public override void Log(LogEventLevel logEventLevel, Exception exception, string message, params object[] args)
        {
        }
    }
}