namespace Snapper.Core
{
    internal interface ISnapshotComparer
    {
        bool CompareSnapshots(object oldSnapshot, object newSnapshot);
    }
}