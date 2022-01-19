# SimpleLambdaLogger

**This is a prerelease version of SimpleLambdaLogger.**

**SimpleLambdaLogger** is a scope-based and buffered logging library for AWS Lambda C# functions that output a single JSON log message to the CloudWatch console after every lambda invocation. Logging frequency is configurable, it can write once after every certain number of invocations. It is very easy to use and set up. It can be used even without configuration.

## Getting started

First, add [NuGet library](https://www.nuget.org/packages/SimpleLambdaLogger) into your project:

```
dotnet add package SimpleLambdaLogger --version 1.0.0-preview002
```

or

```
Install-Package SimpleLambdaLogger -Version 1.0.0-preview002
```


* This is the simplest way to use **SimpleLambdaLogger** without any configuration. The scope has it's own log level and overrides the default log level which is `Information`. The output contains `duration` field which shows execution time of each scope in miliseconds. `contextId` field can be used to track each scope by using AwsRequestId or any other correlation id value.

```csharp
using SimpleLambdaLogger;

public class Function
{
    // input: "Hello World"
    public string FunctionHandler(string input, ILambdaContext context)
    {
        using (var scope = Scope.Begin<Function>(context.AwsRequestId, LogEventLevel.Trace))
        {
            scope.LogTrace(input);
            var result = input?.ToLower();
            scope.LogTrace(result);

            return result;
        }
    }
}
```

```json
{
    "scope": "Function",
    "duration": 2,
    "contextId": "5a02102e-d4b1-4934-b8e6-59ce91e0e8dc",
    "success": true,
    "logs": [
        {
            "level": "Trace",
            "message": "Hello World",
            "timestamp": "2022-01-17T20:15:19.1193613+00:00"
        },
        {
            "level": "Trace",
            "message": "hello world",
            "timestamp": "2022-01-17T20:15:19.8179792+00:00"
        }
    ],
    "scopes": []
}
```
* `success` field depends on the min failure log level. By default, it is `Error`

```csharp
using SimpleLambdaLogger;

public class Function
{
    // for the input: "Hello World"
    public async Task<string> FunctionHandler(string input, ILambdaContext context)
    {
        using (var scope = Scope.Begin<Function>(context.AwsRequestId, LogEventLevel.Trace))
        {
            scope.LogTrace(input);
            var result = input?.ToLower();
            scope.LogError(result);

            return result;
        }
    }
}
```

```json
{
    "scope": "Function",
    "duration": 640,
    "contextId": "053a4e58-c8ea-4dbb-af93-1a81c0f65dac",
    "success": false,
    "logs": [
        {
            "level": "Trace",
            "message": "Hello World",
            "timestamp": "2022-01-17T21:49:13.7835968+00:00"
        },
        {
            "level": "Error",
            "message": "hello world",
            "timestamp": "2022-01-17T21:49:14.4236724+00:00"
        }
    ],
    "scopes": []
}
```

* Nested scopes

```csharp
using SimpleLambdaLogger;

public class Function
{    
    // input: "Hello World"
    public string FunctionHandler(string input, ILambdaContext context)
    {
        using (var scope = Scope.Begin<Function>(context.AwsRequestId, LogEventLevel.Trace))
        {
            scope.LogTrace(input);
            var result = ToLowerCase(input);
            scope.LogTrace(result);

            return result;
        }
    }

    public string ToLowerCase(string input)
    {
        using (var scope = Scope.Begin("ToLowerCase", LogEventLevel.Trace))
        {
            scope.LogTrace(input);
            var result = input?.ToLower();
            scope.LogTrace(result);

            return result;
        }
    }
}
```

```json
{
    "scope": "Function",
    "duration": 639,
    "contextId": "82cca306-15c6-450d-9059-65d64b1c134d",
    "success": true,
    "logs": [
        {
            "level": "Trace",
            "message": "Hello World",
            "timestamp": "2022-01-17T22:37:22.9324228+00:00"
        },
        {
            "level": "Trace",
            "message": "hello world",
            "timestamp": "2022-01-17T22:37:23.5714309+00:00"
        }
    ],
    "scopes": [
        {
            "scope": "Function.ToLowerCase",
            "duration": 479,
            "contextId": "82cca306-15c6-450d-9059-65d64b1c134d",
            "success": true,
            "logs": [
                {
                    "level": "Trace",
                    "message": "Hello World",
                    "timestamp": "2022-01-17T22:37:23.0919952+00:00"
                },
                {
                    "level": "Trace",
                    "message": "hello world",
                    "timestamp": "2022-01-17T22:37:23.5710483+00:00"
                }
            ],
            "scopes": []
        }
    ]
}
```

* Nested scopes with different log levels:

```csharp
using SimpleLambdaLogger;

public class Function
{
    // input: "Hello World"
    public string FunctionHandler(string input, ILambdaContext context)
    {
        using (var scope = Scope.Begin<Function>(context.AwsRequestId, LogEventLevel.Critical))
        {
            scope.LogTrace(input);
            var result = ToLowerCase(input);
            scope.LogTrace(result);

            return result;
        }
    }

    public string ToLowerCase(string input)
    {
        using (var scope = Scope.Begin("ToLowerCase", LogEventLevel.Information))
        {
            scope.LogInformation(input);
            var result = input?.ToLower();
            scope.LogInformation(result);

            return result;
        }
    }
}
```

The inner scope satisfies with the minimum log level:

```json
{
    "scope": "Function",
    "duration": 0,
    "contextId": "94ae51c0-a41d-4010-b6a4-576cb05cafaf",
    "success": true,
    "logs": [
        {
            "level": "Trace",
            "message": "Hello World",
            "timestamp": "2022-01-17T22:53:59.4181365+00:00"
        },
        {
            "level": "Trace",
            "message": "hello world",
            "timestamp": "2022-01-17T22:53:59.4182013+00:00"
        }
    ],
    "scopes": [
        {
            "scope": "Function.ToLowerCase",
            "duration": 0,
            "contextId": "94ae51c0-a41d-4010-b6a4-576cb05cafaf",
            "success": true,
            "logs": [
                {
                    "level": "Information",
                    "message": "Hello World",
                    "timestamp": "2022-01-17T22:53:59.4181869+00:00"
                },
                {
                    "level": "Information",
                    "message": "hello world",
                    "timestamp": "2022-01-17T22:53:59.4181972+00:00"
                }
            ],
            "scopes": []
        }
    ]
}
```
* Logging rate with initial configuration: 

```csharp
public class Function
{
    public Function()
    {
        Scope.Configure(logLevel: LogEventLevel.Trace, loggingRate: 10);
    }
    
    // input: "Hello World"
    public string FunctionHandler(string input, ILambdaContext context)
    {
        using (var scope = Scope.Begin<Function>(context.AwsRequestId))
        {
            scope.LogInformation(input);
            var result = input.ToLower();
            scope.LogInformation(result);
            return result;
        }
    }
}
```

It outputs once for every 10 invocations:

```json
{
    "scope": "Function",
    "duration": 0,
    "contextId": "b1557c4d-42d7-41e3-afaa-471870f6971a",
    "success": true,
    "logs": [
        {
            "level": "Information",
            "message": "Hello World",
            "timestamp": "2022-01-17T22:57:29.6560841+00:00"
        },
        {
            "level": "Information",
            "message": "hello world",
            "timestamp": "2022-01-17T22:57:29.656128+00:00"
        }
    ],
    "scopes": []
}
```

* Multiple asyc scopes:
```csharp
public class Function
{
    // input: "Hello World"
    public async Task<string> FunctionHandler(string input, ILambdaContext context)
    {
        using (var scope = Scope.Begin<Function>(context.AwsRequestId, LogEventLevel.Trace))
        {
            scope.LogTrace(input);
            await Task.WhenAll(ToLower(input), ToUpper(input));
            return input;
        }
    }

    public async Task ToLower(string input)
    {
        using (var scope = Scope.Begin("ToLower"))
        {
            scope.LogTrace(input.ToLower());
        }
    }
    
    public async Task ToUpper(string input)
    {
        using (var scope = Scope.Begin("ToUpper"))
        {
            scope.LogTrace(input.ToUpper());
        }
    }
}
```
```json
{
    "scope": "Function",
    "duration": 680,
    "contextId": "c2b457a8-7e69-45bc-b585-f4a52c95e261",
    "success": true,
    "logs": [
        {
            "level": "Trace",
            "message": "Hello World",
            "timestamp": "2022-01-17T23:26:24.4333478+00:00"
        }
    ],
    "scopes": [
        {
            "scope": "Function.ToLower",
            "duration": 498,
            "contextId": "c2b457a8-7e69-45bc-b585-f4a52c95e261",
            "success": true,
            "logs": [
                {
                    "level": "Trace",
                    "message": "hello world",
                    "timestamp": "2022-01-17T23:26:25.1119557+00:00"
                }
            ],
            "scopes": []
        },
        {
            "scope": "Function.ToUpper",
            "duration": 0,
            "contextId": "c2b457a8-7e69-45bc-b585-f4a52c95e261",
            "success": true,
            "logs": [
                {
                    "level": "Trace",
                    "message": "HELLO WORLD",
                    "timestamp": "2022-01-17T23:26:25.1130251+00:00"
                }
            ],
            "scopes": []
        }
    ]
}
```

## Credits

<a target="_blank" href="https://www.linkedin.com/in/cagri-cakir-8ba45955/"><img src="https://img.shields.io/badge/linkedin-%230077B5.svg?style=for-the-badge&amp;logo=linkedin&amp;logoColor=white" style="max-width: 100%;"></a>
<a target="_blank" href="https://twitter.com/cagrica"><img src="https://img.shields.io/badge/twitter-%231DA1F2.svg?style=for-the-badge&logo=Twitter&logoColor=white" style="max-width: 100%;"></a>
