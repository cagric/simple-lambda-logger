using SimpleLambdaLogger.Events;

namespace SimpleLambdaLogger.Internal;

internal static class Settings
{
    internal static readonly IReadOnlyDictionary<LogEventLevel, string> LogLevelsLookup =
        new Dictionary<LogEventLevel, string>
    {
        {LogEventLevel.Trace, "Trace"},
        {LogEventLevel.Debug, "Debug"},
        {LogEventLevel.Information, "Information"},
        {LogEventLevel.Warning, "Warning"},
        {LogEventLevel.Error, "Error"},
        {LogEventLevel.Critical, "Critical"}
    };
}