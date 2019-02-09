namespace Snapper.Core
{
    public interface ISnapshotComparer
    {
        bool CompareSnapshots(object oldSnapshot, object newSnapshot);
    }
}