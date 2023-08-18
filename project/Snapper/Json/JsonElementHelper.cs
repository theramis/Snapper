using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Snapper.Json;

internal static class JsonElementHelper
{
    public static JsonElement ParseFromString(string jsonString, SnapshotSettings snapshotSettings)
    {
        return JsonSerializer.Deserialize<JsonElement>(jsonString, CreateSerialiserSettings(snapshotSettings));
    }

    public static JsonNode? ParseNodeFromString(string jsonString, SnapshotSettings snapshotSettings)
    {
        return JsonSerializer.Deserialize<JsonNode>(jsonString, CreateSerialiserSettings(snapshotSettings));
    }

    public static JsonElement FromObject(object? obj, SnapshotSettings snapshotSettings)
    {
        if (obj is JsonElement element)
            return element;

        return JsonSerializer.SerializeToElement(obj, CreateSerialiserSettings(snapshotSettings));
    }

    public static string ToString(JsonElement json, SnapshotSettings? snapshotSettings = null)
    {
        return JsonSerializer.Serialize(json, CreateSerialiserSettings(snapshotSettings ?? SnapshotSettings.New()));
    }

    public static string ToString(JsonNode json, SnapshotSettings snapshotSettings)
    {
        return JsonSerializer.Serialize(json, CreateSerialiserSettings(snapshotSettings));
    }

    /// <summary>
    /// <see href="https://stackoverflow.com/questions/60580743/what-is-equivalent-in-jtoken-deepequals-in-system-text-json"/>
    /// </summary>
    public static bool JsonEquals(JsonElement x, JsonElement y)
    {
        if (x.ValueKind != y.ValueKind)
            return false;
        switch (x.ValueKind)
        {
            case JsonValueKind.Null:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Undefined:
                return true;

            case JsonValueKind.Number:
                return x.GetRawText() == y.GetRawText();

            case JsonValueKind.String:
                return x.GetString() == y.GetString();

            case JsonValueKind.Array:
                return x.EnumerateArray().SequenceEqual(y.EnumerateArray());

            case JsonValueKind.Object:
            {
                var xPropertiesUnsorted = x.EnumerateObject().ToList();
                var yPropertiesUnsorted = y.EnumerateObject().ToList();
                if (xPropertiesUnsorted.Count != yPropertiesUnsorted.Count)
                    return false;
                var xProperties = xPropertiesUnsorted.OrderBy(p => p.Name, StringComparer.Ordinal);
                var yProperties = yPropertiesUnsorted.OrderBy(p => p.Name, StringComparer.Ordinal);
                foreach (var (px, py) in xProperties.Zip(yProperties, (l, r) => (l, r)))
                {
                    if (px.Name != py.Name)
                        return false;
                    if (!JsonEquals(px.Value, py.Value))
                        return false;
                }
                return true;
            }

            default:
                throw new JsonException($"Unknown JsonValueKind {x.ValueKind}");
        }
    }

    private static JsonSerializerOptions CreateSerialiserSettings(SnapshotSettings snapshotSettings)
    {
        var serializerSettings = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        SnapshotSettings.GlobalSnapshotSerialiserSettings?.Invoke(serializerSettings);
        snapshotSettings.SnapshotSerialiserSettings?.Invoke(serializerSettings);

        return serializerSettings;
    }
}
