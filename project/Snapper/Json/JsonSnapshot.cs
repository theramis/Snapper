using System.Text.Json.Nodes;
using Snapper.Core;

namespace Snapper.Json;

internal record JsonSnapshot(SnapshotId Id, JsonNode Value)
{
    public bool CompareValues(JsonSnapshot other)
    {
        return JsonNodeHelper.JsonEquals(Value, other.Value);
    }
}

