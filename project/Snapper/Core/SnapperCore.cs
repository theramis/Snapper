using Snapper.Json;

namespace Snapper.Core;

internal class SnapperCore
{
    private readonly ISnapshotStore _snapshotStore;
    private readonly ISnapshotUpdateDecider _snapshotUpdateDecider;

    public SnapperCore(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider)
    {
        _snapshotStore = snapshotStore;
        _snapshotUpdateDecider = snapshotUpdateDecider;
    }

    public SnapResult Snap(JsonSnapshot newSnapshot)
    {
        var existingSnapshot = _snapshotStore.GetSnapshot(newSnapshot.Id);

        if (ShouldUpdateSnapshot(existingSnapshot, newSnapshot))
        {
            _snapshotStore.StoreSnapshot(newSnapshot);
            return SnapResult.SnapshotUpdated(existingSnapshot, newSnapshot);
        }

        if (existingSnapshot == null)
        {
            return SnapResult.SnapshotDoesNotExist(newSnapshot);
        }

        return existingSnapshot.CompareValues(newSnapshot)
            ? SnapResult.SnapshotsMatch(existingSnapshot, newSnapshot)
            : SnapResult.SnapshotsDoNotMatch(existingSnapshot, newSnapshot);
    }

    private bool ShouldUpdateSnapshot(JsonSnapshot? existingSnapshot, JsonSnapshot newSnapshot)
    {
        var snapshotsAreEqual = existingSnapshot != null && existingSnapshot.CompareValues(newSnapshot);
        if (!snapshotsAreEqual && _snapshotUpdateDecider.ShouldUpdateSnapshot())
        {
            return true;
        }

        // Create snapshot if it doesn't currently exist and its not a CI env
        if (existingSnapshot == null)
        {
            return !CiEnvironmentDetector.IsCiEnv();
        }

        return false;
    }
}
