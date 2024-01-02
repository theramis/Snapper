using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Snapper.Json;

internal static class JsonNodeHelper
{
    public static JsonNode ParseFromString(string jsonString, SnapshotSettings snapshotSettings)
    {
        return JsonSerializer.Deserialize<JsonNode>(jsonString, CreateSerialiserSettings(snapshotSettings))
               ?? throw new InvalidOperationException();
    }

    public static JsonNode FromObject(object? obj, SnapshotSettings snapshotSettings)
    {
        if (obj is JsonNode node)
            return node;

        return JsonSerializer.SerializeToNode(obj, CreateSerialiserSettings(snapshotSettings))
               ?? throw new InvalidOperationException();
    }

    public static string ToString(JsonNode json, SnapshotSettings? snapshotSettings = null)
    {
        return JsonSerializer.Serialize(json, CreateSerialiserSettings(snapshotSettings ?? SnapshotSettings.New()));
    }

    public static bool JsonEquals(JsonNode x, JsonNode y)
        => JsonNode.DeepEquals(x, y);

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

    public static bool TryGetValue(this JsonNode node, string propertyName, out JsonNode value)
    {
        var prop = node[propertyName];
        value = prop ?? new JsonObject();
        return prop != null;
    }
}
