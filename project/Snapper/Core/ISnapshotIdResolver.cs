namespace Snapper.Core
{
    public interface ISnapshotIdResolver
    {
        string ResolveSnapshotId(string snapshotName);
    }
}
