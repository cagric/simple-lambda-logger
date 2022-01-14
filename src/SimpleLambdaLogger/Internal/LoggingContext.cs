using System.Runtime.CompilerServices;
using System.Threading;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Scopes;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]

namespace SimpleLambdaLogger.Internal
{
    internal static class LoggingContext
    {
        private static long _invocationCount = 0;
        private static LogEventLevel _minLogLevel = LogEventLevel.Error;
        private static LogEventLevel _minFailureLogLevel = LogEventLevel.Error;
        private static long _loggingRate = 1;
        private static readonly AsyncLocal<DefaultScope> _currentScope = new();
        private static bool _isSilentScope;

        internal static void Initialize(
            LogEventLevel minLogLevel = LogEventLevel.Information,
            LogEventLevel minFailureLogLevel = LogEventLevel.Error,
            long loggingRate = 1)
        {
            _minLogLevel = minLogLevel;
            _minFailureLogLevel = minFailureLogLevel;
            _loggingRate = loggingRate;
        }
    
        internal static IScope CreateScope(string scopeName, string? contextId)
        {
            return CreateScope(scopeName, contextId, _minLogLevel, _minFailureLogLevel);
        }

        internal static IScope CreateScope(string scopeName, string? contextId, LogEventLevel scopeLogLevel)
        {
            return CreateScope(scopeName, contextId, scopeLogLevel, _minFailureLogLevel);
        }
        
        private static IScope CreateScope(string scopeName, string? contextId, LogEventLevel scopeLogLevel, LogEventLevel minFailureLogLevel)
        {
            if (_currentScope.Value == null && _isSilentScope == false)
            {
                _invocationCount++;
            }

            if (_loggingRate != 1 && _invocationCount % _loggingRate != 0)
            {
                var scope = new SilentScope(!_isSilentScope);
                _isSilentScope = true;
                return scope;
            }
            
            _currentScope.Value = new DefaultScope(scopeName, contextId, scopeLogLevel, minFailureLogLevel, _currentScope.Value);
            
            return _currentScope.Value;
        }
        
        internal static void ChangeCurrentScope(DefaultScope scope)
        {
            _currentScope.Value = scope;
        }
        
        internal static void ResetCurrentScope()
        {
            _isSilentScope = false;
            ChangeCurrentScope(null);
        }
    }   
}