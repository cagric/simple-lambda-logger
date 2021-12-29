using System;
using System.Threading;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Internal;
using SimpleLambdaLogger.Scopes;

namespace SimpleLambdaLogger
{
    public class SimpleLogger
    {
        public static void Configure(
            LogEventLevel logLevel = LogEventLevel.Error,
            long loggingRate = 1)
        {
            if (loggingRate < 1)
            {
                throw new ArgumentException(nameof(loggingRate));
            }

            LoggingContext.Initialize(logLevel, loggingRate);
        }

        public static IScope BeginScope<TScope>()
        {
            return LoggingContext.CreateScope(typeof(TScope).Name, null);
        }

        public static IScope BeginScope(string scope)
        {
            return LoggingContext.CreateScope(scope, null);
        }

        public static IScope BeginScope<TScope>(string? contextId)
        {
            return LoggingContext.CreateScope(typeof(TScope).Name, contextId);
        }

        public static IScope BeginScope(string scope, string? contextId)
        {
            return LoggingContext.CreateScope(scope, contextId);
        }

        public static IScope BeginScope<TScope>(LogEventLevel scopeLogLevel)
        {
            return LoggingContext.CreateScope(typeof(TScope).Name, null, scopeLogLevel);
        }

        public static IScope BeginScope(string scope, LogEventLevel scopeLogLevel)
        {
            return LoggingContext.CreateScope(scope, null, scopeLogLevel);
        }

        public static IScope BeginScope<TScope>(string? contextId, LogEventLevel scopeLogLevel)
        {
            return LoggingContext.CreateScope(typeof(TScope).Name, contextId, scopeLogLevel);
        }

        public static IScope BeginScope(string scope, string? contextId, LogEventLevel scopeLogLevel)
        {
            return LoggingContext.CreateScope(scope, contextId, scopeLogLevel);
        }
    }
}