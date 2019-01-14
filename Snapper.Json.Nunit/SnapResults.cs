namespace Snapper.Json.Nunit
{
    internal class SnapResults
    {
        public static SnapResults NoSnapshotPresent()
        {
            return new SnapResults(Result.NoSnapshotPresent);
        }

        public static SnapResults ValueEqualToSnapshot()
        {
            return new SnapResults(Result.ValueEqualToSnapshot);
        }

        public static SnapResults ValueNotEqualToSnapshot(string differences)
        {
            return new SnapResults(Result.ValueNotEqualToSnapshot, differences);
        }

        public Result Result { get; }

        public string Differences { get; }

        private SnapResults(Result result, string differences = null)
        {
            Result = result;
            Differences = differences;
        }
    }

    internal enum Result
    {
        NoSnapshotPresent,
        ValueEqualToSnapshot,
        ValueNotEqualToSnapshot
    }
}
