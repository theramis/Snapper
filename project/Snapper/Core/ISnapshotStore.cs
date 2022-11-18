using Snapper.Json;

namespace Snapper.Core;

internal interface ISnapshotStore
{
    JsonSnapshot? GetSnapshot(SnapshotId snapshotId);

    void StoreSnapshot(JsonSnapshot snapshot);
}
