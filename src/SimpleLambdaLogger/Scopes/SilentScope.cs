using System.Runtime.CompilerServices;
using SimpleLambdaLogger.Events;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace SimpleLambdaLogger.Scopes
{
    internal class SilentScope : BaseScope
    {
        public SilentScope()
        {
        }
        
        public SilentScope(BaseScope parentScope)
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