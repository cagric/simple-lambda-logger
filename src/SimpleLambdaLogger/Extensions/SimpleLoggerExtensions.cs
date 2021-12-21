using System;

namespace SimpleLambdaLogger
{
    public static class SimpleLoggerExtensions
    {
        public static void LogTrace(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Trace, message, args);
        }

        public static void LogTrace(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Trace, message, exception, args);
        }

        public static void LogDebug(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Trace, message, args);
        }

        public static void LogDebug(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Trace, message, exception, args);
        }

        public static void LogInformation(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Information, message, args);
        }

        public static void LogInformation(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Information, message, exception, args);
        }

        public static void LogWarning(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Warning, message, args);
        }

        public static void LogWarning(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Warning, message, exception, args);
        }

        public static void LogError(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Error, message, args);
        }

        public static void LogError(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Error, message, exception, args);
        }

        public static void LogCritical(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Critical, message, exception, args);
        }

        public static void LogCritical(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Critical, message, args);
        }
    }
}