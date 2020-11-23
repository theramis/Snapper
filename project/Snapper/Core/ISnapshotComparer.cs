namespace Snapper.Core
{
    internal interface ISnapshotComparer
    {
        SnapResult CompareSnapshots(object oldSnapshot, object newSnapshot);
    }
}
