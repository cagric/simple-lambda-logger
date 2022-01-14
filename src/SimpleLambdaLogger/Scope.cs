using System;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Scopes;
using static SimpleLambdaLogger.Internal.LoggingContext;

namespace SimpleLambdaLogger
{
    public static class Scope
    {
        public static void Configure(
            LogEventLevel logLevel = LogEventLevel.Information,
            LogEventLevel minFailureLogLevel = LogEventLevel.Error,
            long loggingRate = 1)
        {
            if (loggingRate < 1)
            {
                throw new ArgumentException(nameof(loggingRate));
            }

            Initialize(
                logLevel, 
                minFailureLogLevel,
                loggingRate
                );
        }

        public static IScope Begin<TScope>() => CreateScope(typeof(TScope).Name, null);

        public static IScope Begin(string scope) => CreateScope(scope, null);

        public static IScope Begin(string scope, LogEventLevel scopeLogLevel) => CreateScope(scope, null, scopeLogLevel);

        public static IScope Begin<TScope>(string? contextId) => CreateScope(typeof(TScope).Name, contextId);

        public static IScope Begin(string scope, string? contextId) => CreateScope(scope, contextId);

        public static IScope Begin<TScope>(string? contextId, LogEventLevel scopeLogLevel) => CreateScope(typeof(TScope).Name, contextId, scopeLogLevel);

        public static IScope Begin(string scope, string? contextId, LogEventLevel scopeLogLevel) => CreateScope(scope, contextId, scopeLogLevel);
    }
}