namespace Snapper.Core
{
    public class AlwaysFalseSnapshotUpdateDecider : ISnapshotUpdateDecider
    {
        public bool ShouldUpdateSnapshot() => false;
    }
}
