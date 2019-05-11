namespace Snapper.Core
{
    internal interface ISnapshotUpdateDecider
    {
        bool ShouldUpdateSnapshot();
    }
}
