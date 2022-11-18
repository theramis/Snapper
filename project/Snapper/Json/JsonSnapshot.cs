using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json;

internal record JsonSnapshot(SnapshotId Id, JObject Value)
{
    public bool CompareValues(JsonSnapshot other)
    {
        return JToken.DeepEquals(Value, other.Value);
    }
}

