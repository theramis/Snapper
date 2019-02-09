namespace Snapper.Core
{
    public interface ISnapshotStore
    {
        object GetSnapshot(string snapshotId);

        void StoreSnapshot(string snapshotId, object snapshot);
    }
}
