namespace Snapper.Core
{
    public class SnapperCore
    {
        private readonly ISnapStore _snapStore;
        private readonly ISnapUpdateDecider _snapUpdateDecider;
        private readonly ISnapComparer _snapComparer;
        private readonly ISnapIdResolver _snapIdResolver;

        public SnapperCore(ISnapStore snapStore, ISnapUpdateDecider snapUpdateDecider,
            ISnapComparer snapComparer, ISnapIdResolver snapIdResolver)
        {
            _snapStore = snapStore;
            _snapUpdateDecider = snapUpdateDecider;
            _snapComparer = snapComparer;
            _snapIdResolver = snapIdResolver;
        }

        public SnapResult Snap(string snapName, object newSnapshot)
        {
            var snapId = _snapIdResolver.ResolveSnapId(snapName);
            var currentSnapshot = _snapStore.GetSnap(snapId);
            var areSnapshotsEqual = _snapComparer.Compare(currentSnapshot, newSnapshot);

            if (!areSnapshotsEqual && _snapUpdateDecider.ShouldUpdateSnap())
            {
                _snapStore.StoreSnap(snapId, newSnapshot);
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
