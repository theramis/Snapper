namespace Snapper.Core
{
    internal interface ISnapshotHandler
    {
        SnapResult Snap(SnapshotId id, object newSnapshot);
    }

}
