using System.Runtime.CompilerServices;
using System.Threading;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Formatters;
using SimpleLambdaLogger.Scopes;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]

namespace SimpleLambdaLogger.Internal
{
    internal static class LoggingContext
    {
        private static long _invocationCount = 0;
        private static LogEventLevel _minLogLevel = LogEventLevel.Error;
        private static long _loggingRate = 1;
        private static readonly AsyncLocal<BaseScope> _currentScope = new();

        internal static void Initialize(
            LogEventLevel minLogLevel = LogEventLevel.Error,
            long loggingRate = 1)
        {
            _minLogLevel = minLogLevel;
            _loggingRate = loggingRate;
        }
    
        internal static IScope CreateScope(string scopeName, string? contextId)
        {
            return CreateScope(scopeName, contextId, _minLogLevel);
        }

        internal static IScope CreateScope(string scopeName, string? contextId, LogEventLevel scopeLogLevel)
        {
            if (_currentScope.Value == null)
            {
                _invocationCount++;
            }
            
            BaseScope scope = _loggingRate != 1 && _invocationCount % _loggingRate != 0
                ? new SilentScope(_currentScope.Value)
                : new DefaultScope(new JsonFormatter() ,scopeName, contextId, scopeLogLevel, _currentScope.Value);
            
            _currentScope.Value = scope;
            
            return scope;
        }
        
        internal static void ChangeCurrentScope(BaseScope scope)
        {
            _currentScope.Value = scope;
        }
        
        internal static void ResetCurrentScope()
        {
            ChangeCurrentScope(null);
        }
    }   
}