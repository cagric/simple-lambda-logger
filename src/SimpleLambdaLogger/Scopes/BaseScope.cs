using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleLambdaLogger.Events;

[assembly: InternalsVisibleTo("SimpleLambdaLogger.Unit.Tests")]
namespace SimpleLambdaLogger.Scopes
{
    internal abstract class BaseScope : IScope
    {
        public abstract void Log(LogEventLevel logEventLevel, string message, params object[] args);

        public abstract void Log(LogEventLevel logEventLevel, Exception exception, string message, params object[] args);

        public abstract void Dispose();
    }
}