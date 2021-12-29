
using System;
using System.Linq;
using System.Text.Json;
using BenchmarkDotNet.Running;
using SimpleLambdaLogger.Benchmarks;

namespace SimpleLambdaLogger.Benchmarks
{
    class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<LoggingRateBenchmarks>();
            BenchmarkRunner.Run<LogLevelBenchmarks>();
        }
    }
}