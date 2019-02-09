namespace Snapper
{
    public static class SnapperExtensions
    {
        public static void ShouldMatchSnapshot(this object snapshot)
        {
            var snapper = SnapperFactory.CreateJsonSnapper();
            snapper.MatchSnapshot(snapshot);
        }

        public static void ShouldMatchSnapshot(this object snapshot, string snapshotName)
        {
            var snapper = SnapperFactory.CreateJsonSnapper();
            snapper.MatchSnapshot(snapshot, snapshotName);
        }
    }
}
