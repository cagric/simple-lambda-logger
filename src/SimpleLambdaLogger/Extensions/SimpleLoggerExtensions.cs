using System;
using SimpleLambdaLogger.Events;
using SimpleLambdaLogger.Scopes;

namespace SimpleLambdaLogger
{
    public static class SimpleLoggerExtensions
    {
        public static void LogTrace(this IScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Trace, message, args);
        }

        public static void LogTrace(this IScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Trace, exception, message, args);
        }

        public static void LogDebug(this IScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Trace, message, args);
        }

        public static void LogDebug(this IScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Trace, exception, message, args);
        }

        public static void LogInformation(this IScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Information, message, args);
        }

        public static void LogInformation(this IScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Information, exception, message, args);
        }

        public static void LogWarning(this IScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Warning, message, args);
        }

        public static void LogWarning(this IScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Warning, exception, message, args);
        }

        public static void LogError(this IScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Error, message, args);
        }

        public static void LogError(this IScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Error, exception, message, args);
        }

        public static void LogCritical(this IScope logger, Exception exception, string message,
            params object[] args)
        {
            logger.Log(LogEventLevel.Critical, exception, message, args);
        }

        public static void LogCritical(this IScope logger, string message, params object[] args)
        {
            logger.Log(LogEventLevel.Critical, message, args);
        }
    }
}