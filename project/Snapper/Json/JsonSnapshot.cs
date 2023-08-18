using System.Text.Json;
using Snapper.Core;

namespace Snapper.Json;

internal record JsonSnapshot(SnapshotId Id, JsonElement Value)
{
    public bool CompareValues(JsonSnapshot other)
    {
        return JsonElementHelper.JsonEquals(Value, other.Value);
    }
}

