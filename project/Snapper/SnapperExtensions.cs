namespace Snapper
{
    public static class SnapperExtensions
    {
        /// <summary>
        ///     Compares the provided object with the stored snapshot.
        /// </summary>
        /// <param name="snapshot">The object to compare with the stored snapshot</param>
        public static void ShouldMatchSnapshot(this object snapshot)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot);
        }

        /// <summary>
        ///     Compares the provided object with the stored snapshot.
        ///     Takes in a unique instance name for use in theory tests.
        /// </summary>
        /// <param name="snapshot">The object to compare with the stored snapshot</param>
        /// <param name="uniqueInstanceName">Used to determine which snapshot the test is referring to.</param>
        public static void ShouldMatchSnapshot(this object snapshot, string uniqueInstanceName)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot, uniqueInstanceName);
        }

        /// <summary>
        ///     Compares the provided object with an inline snapshot.
        ///     Use when snapshots are simple and small.
        /// </summary>
        /// <param name="snapshot">The object to compare with the inline snapshot</param>
        /// <param name="expectedSnapshot">The inline snapshot to which the object is compared with</param>
        public static void ShouldMatchInlineSnapshot(this object snapshot, object expectedSnapshot)
        {
            var snapper = SnapperFactory.GetJsonInlineSnapper(expectedSnapshot);
            snapper.MatchSnapshot(snapshot);
        }
    }
}
