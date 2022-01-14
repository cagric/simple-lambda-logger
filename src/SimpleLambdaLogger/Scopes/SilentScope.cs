using System;
using System.Runtime.CompilerServices;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Internal;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace SimpleLambdaLogger.Scopes
{
    internal class SilentScope : BaseScope
    {
        private readonly bool _isParent;

        public SilentScope(bool isParent = false)
        {
            _isParent = isParent;
        }

        public override void Dispose()
        {
            if (_isParent)
            {
                LoggingContext.ResetCurrentScope();
            }
        }

        public override void Log(LogEventLevel logEventLevel, string message, params object[] args)
        {
        }

        public override void Log(LogEventLevel logEventLevel, Exception exception, string message, params object[] args)
        {
        }
    }
}