using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using SimpleLambdaLogger.Events;

namespace SimpleLambdaLogger.Internal
{
    internal static class Settings
    {
        internal static readonly JsonSerializerOptions SerializationOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true,
            Converters =
            {
                new JsonStringEnumConverter(null)
            }
        };
    }
}

