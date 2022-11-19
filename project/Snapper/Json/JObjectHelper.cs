using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Snapper.Json;

internal static class JObjectHelper
{
    public static JObject ParseFromString(string jsonString, SnapshotSettings snapshotSettings)
    {
        return JsonConvert.DeserializeObject(jsonString, CreateSerialiserSettings(snapshotSettings)) as JObject
               ?? throw new InvalidOperationException("Error when parsing snapshot.");
    }

    public static JObject FromObject(object? obj, SnapshotSettings snapshotSettings)
    {
        JObject result;

        if (obj is JObject jObject)
        {
            result = jObject;
        }
        else
        {
            result = JObject.FromObject(obj, JsonSerializer.Create(CreateSerialiserSettings(snapshotSettings)));
        }

        // Converting to a string first and reparsing because newtonsoft interprets the object
        // differently from string vs object.
        // (e.g. datetime is treated as datetime with object but as a string when parsed from string)
        return ParseFromString(result.ToString(), snapshotSettings);
    }

    private static JsonSerializerSettings CreateSerialiserSettings(SnapshotSettings snapshotSettings)
    {
        var serializerSettings = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.None,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore
        };

        SnapshotSettings.GlobalSnapshotSerialiserSettings?.Invoke(serializerSettings);
        snapshotSettings.SnapshotSerialiserSettings?.Invoke(serializerSettings);

        return serializerSettings;
    }
}
