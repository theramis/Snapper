using Snapper.Json;

namespace Snapper.Core;

// TODO is this still needed?
internal class SnapperCore
{
    private readonly ISnapshotStore _snapshotStore;
    private readonly ISnapshotUpdateDecider _snapshotUpdateDecider;

    protected SnapperCore(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider)
    {
        _snapshotStore = snapshotStore;
        _snapshotUpdateDecider = snapshotUpdateDecider;
    }

    protected SnapResult Snap(JsonSnapshot newSnapshot)
    {
        var currentSnapshot = _snapshotStore.GetSnapshot(newSnapshot.Id);

        if (ShouldUpdateSnapshot(currentSnapshot, newSnapshot))
        {
            _snapshotStore.StoreSnapshot(newSnapshot);
            return SnapResult.SnapshotUpdated(currentSnapshot, newSnapshot);
        }

        if (currentSnapshot == null)
        {
            return SnapResult.SnapshotDoesNotExist(newSnapshot);
        }

        return currentSnapshot.CompareValues(newSnapshot)
            ? SnapResult.SnapshotsMatch(currentSnapshot, newSnapshot)
            : SnapResult.SnapshotsDoNotMatch(currentSnapshot, newSnapshot);
    }

    private bool ShouldUpdateSnapshot(JsonSnapshot? currentSnapshot, JsonSnapshot newSnapshot)
    {
        var snapshotsAreEqual = currentSnapshot != null && currentSnapshot.CompareValues(newSnapshot);
        if (!snapshotsAreEqual && _snapshotUpdateDecider.ShouldUpdateSnapshot())
        {
            return true;
        }

        // Create snapshot if it doesn't currently exist and its not a CI env
        if (currentSnapshot == null)
        {
            return !CiEnvironmentDetector.IsCiEnv();
        }

        return false;
    }
}
