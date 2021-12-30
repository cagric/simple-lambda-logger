# SimpleLambdaLogger

SimpleLambdaLogger is scope-based and buffered logging library for AWS Lambda C# functions that output a single JSON log message to the CloudWatch console after every lambda invocations. Logging frequency is configurable, it can write once after every certain number of invocations. It is very easy to use and set up. It can be used even without configuration.

## Getting started

* This is the simplest way to use SimpleLambdaLogger without any configuration. The scope has it's own log level and overrides the default log level which is `Information`. The output contains `duration` field which shows execution time of each scope in miliseconds. `contextId` field can be used to track each scope by using AwsRequestId or any other correlation id value.

```csharp
using SimpleLambdaLogger;

public class Function
{
    public string FunctionHandler(
        string input, ILambdaContext context)
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
    "duration": 1,
    "contextId": "45134dc7-e432-488b-b8fe-f6622f3cd373",
    "logs": [
        {
            "level": "Trace",
            "created": "12/29/2021 9:22:11 PM +00:00",
            "message": "Hello World"
        },
        {
            "level": "Trace",
            "created": "12/29/2021 9:22:11 PM +00:00",
            "message": "hello world"
        }
    ]
}
```

* Nested scopes

```csharp
using SimpleLambdaLogger;

public class Function
{    
    // for the input: "Hello World"
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
    "duration": 138,
    "contextId": "39e09942-0360-46ec-a5e6-a33a86d7943b",
    "logs": [
        {
            "level": "Trace",
            "created": "12/29/2021 9:37:09 PM +00:00",
            "message": "Hello World"
        },
        {
            "level": "Trace",
            "created": "12/29/2021 9:37:09 PM +00:00",
            "message": "hello world"
        }
    ],
    "childScopes": [
        {
            "scope": "ToLowerCase",
            "duration": 99,
            "logs": [
                {
                    "level": "Trace",
                    "created": "12/29/2021 9:37:09 PM +00:00",
                    "message": "Hello World"
                },
                {
                    "level": "Trace",
                    "created": "12/29/2021 9:37:09 PM +00:00",
                    "message": "hello world"
                }
            ]
        }
    ]
}
```

* Nested scopes with different log levels:

```csharp
using SimpleLambdaLogger;

public class Function
{
    // for the input: "Hello World"
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
    "contextId": "35b775a2-7786-45a4-83e1-0c28e58b25ec",
    "logs": [
        {
            "level": "Trace",
            "created": "12/29/2021 10:03:11 PM +00:00",
            "message": "Hello World"
        },
        {
            "level": "Trace",
            "created": "12/29/2021 10:03:11 PM +00:00",
            "message": "hello world"
        }
    ],
    "childScopes": [
        {
            "scope": "ToLowerCase",
            "duration": 0,
            "logs": [
                {
                    "level": "Information",
                    "created": "12/29/2021 10:03:11 PM +00:00",
                    "message": "Hello World"
                },
                {
                    "level": "Information",
                    "created": "12/29/2021 10:03:11 PM +00:00",
                    "message": "hello world"
                }
            ]
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
    
    // for the input: "Hello World"
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
    "duration": 3,
    "contextId": "287d91a0-ad07-4a98-8b42-1d92ca44f2f0",
    "logs": [
        {
            "level": "Information",
            "created": "12/29/2021 10:08:53 PM +00:00",
            "message": "Hello World"
        },
        {
            "level": "Information",
            "created": "12/29/2021 10:08:53 PM +00:00",
            "message": "hello world"
        }
    ]
}
```

* Multiple asyc scopes:
```csharp
public class Function
{
    // for the input: "Hello World"
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
    "duration": 142,
    "contextId": "540eced3-d78a-4ba2-af42-c4360c72744e",
    "logs": [
        {
            "level": "Trace",
            "created": "12/29/2021 10:34:32 PM +00:00",
            "message": "Hello World"
        }
    ],
    "childScopes": [
        {
            "scope": "ToLower",
            "duration": 103,
            "logs": [
                {
                    "level": "Trace",
                    "created": "12/29/2021 10:34:32 PM +00:00",
                    "message": "hello world"
                }
            ]
        },
        {
            "scope": "ToUpper",
            "duration": 0,
            "logs": [
                {
                    "level": "Trace",
                    "created": "12/29/2021 10:34:32 PM +00:00",
                    "message": "HELLO WORLD"
                }
            ]
        }
    ]
}
```