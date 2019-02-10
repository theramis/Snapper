namespace Snapper.Core
{
    internal interface ISnapshotStore
    {
        object GetSnapshot(SnapshotId snapshotId);

        void StoreSnapshot(SnapshotId snapshotId, object snapshot);
    }
}
