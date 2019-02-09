namespace Snapper.Core
{
    public class SnapperCore
    {
        private readonly ISnapshotStore _snapshotStore;
        private readonly ISnapshotUpdateDecider _snapshotUpdateDecider;
        private readonly ISnapshotComparer _snapshotComparer;

        public SnapperCore(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider,
            ISnapshotComparer snapshotComparer)
        {
            _snapshotStore = snapshotStore;
            _snapshotUpdateDecider = snapshotUpdateDecider;
            _snapshotComparer = snapshotComparer;
        }

        public SnapResult Snap(string snapshotId, object newSnapshot)
        {
            var currentSnapshot = _snapshotStore.GetSnapshot(snapshotId);
            var areSnapshotsEqual = currentSnapshot != null
                                    && _snapshotComparer.CompareSnapshots(currentSnapshot, newSnapshot);

            if (!areSnapshotsEqual && _snapshotUpdateDecider.ShouldUpdateSnapshot())
            {
                _snapshotStore.StoreSnapshot(snapshotId, newSnapshot);
                return SnapResult.SnapshotUpdated(currentSnapshot, newSnapshot);
            }

            if (currentSnapshot == null)
            {
                return SnapResult.SnapshotDoesNotExist(newSnapshot);
            }

            return areSnapshotsEqual
                ? SnapResult.SnapshotsMatch(currentSnapshot, newSnapshot)
                : SnapResult.SnapshotsDoNotMatch(currentSnapshot, newSnapshot);
        }
    }
}
