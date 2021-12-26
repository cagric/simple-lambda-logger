using System;
using System.Threading;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Formatters;
using SimpleLambdaLogger.Scopes;

namespace SimpleLambdaLogger
{
    public static class SimpleLogger
    {
        internal static readonly AsyncLocal<BaseScope> CurrentScope = new();

        private static long _invocationCount = 0;

        private static long _loggingRate = 1;

        private static LogEventLevel _minLogLevel = LogEventLevel.Error;

        public static void Configure(
            LogEventLevel logLevel,
            long loggingRate = 1)
        {
            if (loggingRate < 1)
            {
                throw new ArgumentException(nameof(loggingRate));
            }

            _minLogLevel = logLevel;
            _loggingRate = loggingRate;
        }

        private static ILoggerScope CreateScope(string scopeName, string? contextId, LogEventLevel scopeLogLevel)
        {
            if (CurrentScope.Value == null)
            {
                _invocationCount++;
            }

            BaseScope scope = _loggingRate != 1 && _invocationCount % _loggingRate != 0
                ? new SilentScope(CurrentScope.Value)
                : new DefaultScope(new JsonLogFormatter() ,scopeName, contextId, scopeLogLevel, CurrentScope.Value);
            CurrentScope.Value = scope;

            return scope;
        }

        public static ILoggerScope BeginScope<TScope>()
        {
            return CreateScope(typeof(TScope).Name, null, _minLogLevel);
        }

        public static ILoggerScope BeginScope(string scope)
        {
            return CreateScope(scope, null, _minLogLevel);
        }

        public static ILoggerScope BeginScope<TScope>(string? contextId)
        {
            return CreateScope(typeof(TScope).Name, contextId, _minLogLevel);
        }

        public static ILoggerScope BeginScope(string scope, string? contextId)
        {
            return CreateScope(scope, contextId, _minLogLevel);
        }

        public static ILoggerScope BeginScope<TScope>(LogEventLevel scopeLogLevel)
        {
            return CreateScope(typeof(TScope).Name, null, scopeLogLevel);
        }

        public static ILoggerScope BeginScope(string scope, LogEventLevel scopeLogLevel)
        {
            return CreateScope(scope, null, scopeLogLevel);
        }

        public static ILoggerScope BeginScope<TScope>(string? contextId, LogEventLevel scopeLogLevel)
        {
            return CreateScope(typeof(TScope).Name, contextId, scopeLogLevel);
        }

        public static ILoggerScope BeginScope(string scope, string? contextId, LogEventLevel scopeLogLevel)
        {
            return CreateScope(scope, contextId, scopeLogLevel);
        }
    }
}