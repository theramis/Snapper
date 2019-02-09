namespace Snapper.Core
{
    public interface ISnapshotUpdateDecider
    {
        bool ShouldUpdateSnapshot();
    }
}
