namespace Snapper.Core
{
    internal class SnapperCore
    {
        private readonly ISnapshotStore _snapshotStore;
        private readonly ISnapshotUpdateDecider _snapshotUpdateDecider;
        private readonly ISnapshotComparer _snapshotComparer;

        protected SnapperCore(ISnapshotStore snapshotStore, ISnapshotUpdateDecider snapshotUpdateDecider,
            ISnapshotComparer snapshotComparer)
        {
            _snapshotStore = snapshotStore;
            _snapshotUpdateDecider = snapshotUpdateDecider;
            _snapshotComparer = snapshotComparer;
        }

        protected SnapResult Snap(SnapshotId snapshotId, object newSnapshot)
        {
            var currentSnapshot = _snapshotStore.GetSnapshot(snapshotId);
            
            if (ShouldUpdateSnapshot(currentSnapshot, newSnapshot))
            {
                _snapshotStore.StoreSnapshot(snapshotId, newSnapshot);
                return SnapResult.SnapshotUpdated(currentSnapshot, newSnapshot);
            }

            if (currentSnapshot == null)
            {
                return SnapResult.SnapshotDoesNotExist(newSnapshot);
            }

            return _snapshotComparer.CompareSnapshots(currentSnapshot, newSnapshot)
                ? SnapResult.SnapshotsMatch(currentSnapshot, newSnapshot)
                : SnapResult.SnapshotsDoNotMatch(currentSnapshot, newSnapshot);
        }

        private bool ShouldUpdateSnapshot(object currentSnapshot, object newSnapshot)
        {
            // Create snapshot if it doesn't currently exist and its not a CI env
            if (currentSnapshot == null)
            {
                return !CiEnvironmentDetector.IsCiEnv();
            }

            return !_snapshotComparer.CompareSnapshots(currentSnapshot, newSnapshot)
                && _snapshotUpdateDecider.ShouldUpdateSnapshot();
        }
    }
}
