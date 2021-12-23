using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleLambdaLogger.Unit.Tests.Data
{
    public class SilentLoggerTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { LogEventLevel.Critical, new Exception("exception"), "log message", null };
            yield return new object[] { LogEventLevel.Information, null, "log message", null };
            yield return new object[] { LogEventLevel.Trace, new Exception("exception"), null, "param1", "param2" };
            yield return new object[] { LogEventLevel.Error, null, null};
            yield return new object[] { LogEventLevel.Debug, new Exception("exception"), "log message", null };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}