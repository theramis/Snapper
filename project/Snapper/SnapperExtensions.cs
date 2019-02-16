namespace Snapper
{
    public static class SnapperExtensions
    {
        public static void ShouldMatchSnapshot(this object snapshot)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot);
        }

        // TODO figure out if this should be something supported
        public static void ShouldMatchSnapshot(this object snapshot, string snapshotName)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot, snapshotName);
        }

        // TODO figure out if XUnitSnapper should exist
    }
}
