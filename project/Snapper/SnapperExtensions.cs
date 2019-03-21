namespace Snapper
{
    public static class SnapperExtensions
    {
        public static void ShouldMatchSnapshot(this object snapshot)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot);
        }

        // TODO figure out a better name
        public static void TheoryTestShouldMatchSnapshot(this object snapshot, string uniqueInstanceName)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot, uniqueInstanceName);
        }

        // TODO implement inline snapshot matching

        // TODO figure out if XUnitSnapper should exist
    }
}
