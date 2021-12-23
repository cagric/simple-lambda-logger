
using System;
using System.Linq;
using System.Text.Json;
using BenchmarkDotNet.Running;
using SimpleLambdaLogger.Benchmarks;

var summary = BenchmarkRunner.Run<DefaultLoggingRate>();