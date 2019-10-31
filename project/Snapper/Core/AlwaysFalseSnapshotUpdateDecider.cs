namespace Snapper.Core
{
    internal class AlwaysFalseSnapshotUpdateDecider : ISnapshotUpdateDecider
    {
        public bool ShouldUpdateSnapshot() => false;
    }
}
