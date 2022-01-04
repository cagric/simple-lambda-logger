using System;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Internal;
using SimpleLambdaLogger.Scopes;

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

            LoggingContext.Initialize(
                logLevel, 
                minFailureLogLevel,
                loggingRate
                );
        }

        public static IScope Begin<TScope>()
        {
            return LoggingContext.CreateScope(typeof(TScope).Name, null);
        }

        public static IScope Begin(string scope)
        {
            return LoggingContext.CreateScope(scope, null);
        }

        public static IScope Begin<TScope>(string? contextId)
        {
            return LoggingContext.CreateScope(typeof(TScope).Name, contextId);
        }

        public static IScope Begin(string scope, string? contextId)
        {
            return LoggingContext.CreateScope(scope, contextId);
        }
        
        public static IScope Begin(string scope, string? contextId, LogEventLevel scopeLogLevel)
        {
            return LoggingContext.CreateScope(scope, contextId, scopeLogLevel);
        }
    }
}