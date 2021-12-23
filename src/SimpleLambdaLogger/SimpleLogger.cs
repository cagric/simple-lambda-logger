using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace SimpleLambdaLogger
{
    public static class SimpleLogger
    {
        internal static readonly JsonSerializerOptions SerializationOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        internal static readonly AsyncLocal<BaseLoggerScope> CurrentScope = new();

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

            BaseLoggerScope scope = _loggingRate != 1 && _invocationCount % _loggingRate != 0
                ? new SilentLoggerScope(CurrentScope.Value)
                : new LoggerScope(scopeName, contextId, scopeLogLevel, CurrentScope.Value);
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