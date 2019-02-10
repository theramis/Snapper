namespace Snapper.Core
{
    internal interface ISnapshotIdResolver
    {
        SnapshotId ResolveSnapshotId(string snapshotName);
    }
}
