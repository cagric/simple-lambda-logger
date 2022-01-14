using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SimpleLambdaLogger.Events;

namespace SimpleLambdaLogger.Scopes
{
    public interface IScope : IDisposable
    {
        void Log(LogEventLevel logEventLevel, string message, params object[] args);

        void Log(LogEventLevel logEventLevel, Exception exception, string message, params object[] args);
    }
}