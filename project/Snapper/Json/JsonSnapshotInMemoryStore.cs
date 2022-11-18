using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json;

internal class JsonSnapshotInMemoryStore : ISnapshotStore
{
    private readonly JObject _snapshot;

    public JsonSnapshotInMemoryStore(JsonSnapshotSanitiser jsonSnapshotSanitiser,
        SnapshotSettings snapshotSettings, object snapshot)
    {
        _snapshot = JObjectHelper.FromObject(jsonSnapshotSanitiser.SanitiseSnapshot(snapshot),
            snapshotSettings);
    }

    public JsonSnapshot GetSnapshot(SnapshotId snapshotId)
        => new JsonSnapshot(snapshotId, _snapshot);

    public void StoreSnapshot(JsonSnapshot snapshot)
    {}
}
