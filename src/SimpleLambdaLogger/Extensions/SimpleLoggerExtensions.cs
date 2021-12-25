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
            logger.Log(LogEventLevel.Trace, exception, message, args);
        }

        public static void LogDebug(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Trace, message, args);
        }

        public static void LogDebug(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Trace, exception, message, args);
        }

        public static void LogInformation(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Information, message, args);
        }

        public static void LogInformation(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Information, exception, message, args);
        }

        public static void LogWarning(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Warning, message, args);
        }

        public static void LogWarning(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Warning, exception, message, args);
        }

        public static void LogError(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Error, message, args);
        }

        public static void LogError(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Error, exception, message, args);
        }

        public static void LogCritical(this ILoggerScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Critical, exception, message, args);
        }

        public static void LogCritical(this ILoggerScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Critical, message, args);
        }
    }
}