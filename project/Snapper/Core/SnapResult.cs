namespace Snapper.Core
{
    public class SnapResult
    {
        public SnapResultStatus Status { get; }
        public object OldSnapshot { get; }
        public object NewSnapshot { get; }

        private SnapResult(SnapResultStatus status, object oldSnapshot, object newSnapshot)
        {
            Status = status;
            OldSnapshot = oldSnapshot;
            NewSnapshot = newSnapshot;
        }

        internal static SnapResult SnapshotDoesNotExist(object newSnapshot)
            => new SnapResult(SnapResultStatus.SnapshotDoesNotExist, null, newSnapshot);

        internal static SnapResult SnapshotsMatch(object oldSnapshot, object newSnapshot)
            => new SnapResult(SnapResultStatus.SnapshotsMatch, oldSnapshot, newSnapshot);

        internal static SnapResult SnapshotsDoNotMatch(object oldSnapshot, object newSnapshot)
            => new SnapResult(SnapResultStatus.SnapshotsDoNotMatch, oldSnapshot, newSnapshot);

        internal static SnapResult SnapshotUpdated(object oldSnapshot, object newSnapshot)
            => new SnapResult(SnapResultStatus.SnapshotUpdated, oldSnapshot, newSnapshot);
    }
}
