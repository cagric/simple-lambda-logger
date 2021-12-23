using BenchmarkDotNet.Attributes;

namespace SimpleLambdaLogger.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(launchCount: 10, warmupCount: 10, targetCount: 10, invocationCount: 1000)]
    public class DefaultLoggingRate
    {
        [Benchmark(Description = "Single scope with log level lower than min log level", Baseline = true)]
        public void SingleScopeLogsWithLowLogLevel()
        {
            using (var scope = SimpleLogger.BeginScope("test scope",LogEventLevel.Error))
            {
                scope.Log(LogEventLevel.Trace, "log message");
            }
        }
        
        [Benchmark(Description = "Nested scopes with log level lower than min log level")]
        public void LogsWithLowLogLevel()
        {
            using (var scope = SimpleLogger.BeginScope("test scope",LogEventLevel.Error))
            {
                scope.Log(LogEventLevel.Trace, "log message");
                
                using (var scope2 = SimpleLogger.BeginScope("inner scope",LogEventLevel.Error))
                {
                    scope2.Log(LogEventLevel.Information, "log message");
                    
                    using (var scope3 = SimpleLogger.BeginScope("inner scope",LogEventLevel.Error))
                    {
                        scope3.Log(LogEventLevel.Debug, "log message");
                    }
                }
            }
        }
    }
}