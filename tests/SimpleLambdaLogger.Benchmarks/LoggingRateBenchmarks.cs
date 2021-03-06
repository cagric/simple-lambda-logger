using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SimpleLambdaLogger.Events;

namespace SimpleLambdaLogger.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(launchCount: 2, warmupCount: 3, targetCount: 4, invocationCount: 10000,
        runtimeMoniker: RuntimeMoniker.NetCoreApp31)]
    public class LoggingRateBenchmarks
    {
        public LoggingRateBenchmarks()
        {
            Scope.Configure(loggingRate: 10, logLevel: LogEventLevel.Error);
        }
        
        [Benchmark(Description = "Single scope with log level lower than min log level")]
        public void SingleScopeLowLogLevel()
        {
            using (var scope = Scope.Begin("test scope"))
            {
                scope.Log(LogEventLevel.Information, "log message");
            }
        }
        
        [Benchmark(Description = "3 Nested scopes with log level lower than min log level")]
        public void NestedScopesLowLogLevel()
        {
            using (var scope = Scope.Begin("test scope"))
            {
                scope.Log(LogEventLevel.Trace, "log message");
                
                using (var scope2 = Scope.Begin("inner scope"))
                {
                    scope2.Log(LogEventLevel.Information, "log message");
                    
                    using (var scope3 = Scope.Begin("inner scope"))
                    {
                        scope3.Log(LogEventLevel.Debug, "log message");
                    }
                }
            }
        }
        
        [Benchmark(Description = "Single scope with log level higher than min log level")]
        public void SingleScopeHighLogLevel()
        {
            using (var scope = Scope.Begin("test scope"))
            {
                scope.Log(LogEventLevel.Error, "log message");
            }
        }

        [Benchmark(Description = "3 Nested scopes with log level higher than min log level")]
        public void NestedScopesHighLogLevel()
        {
            using (var scope = Scope.Begin("test scope"))
            {
                scope.Log(LogEventLevel.Trace, "log message");

                using (var scope2 = Scope.Begin("inner scope"))
                {
                    scope2.Log(LogEventLevel.Error, "log message");

                    using (var scope3 = Scope.Begin("inner scope"))
                    {
                        scope3.Log(LogEventLevel.Debug, "log message");
                    }
                }
            }
        }
    }
}