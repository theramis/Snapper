namespace Snapper
{
    public static class SnapperExtensions
    {
        public static void ShouldMatchSnapshot(this object snapshot)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot);
        }

        public static void ShouldMatchSnapshot(this object snapshot, string snapshotName)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot, snapshotName);
        }
    }
}
