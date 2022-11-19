namespace Snapper;

public static class SnapperExtensions
{
    /// <summary>
    ///     Compares the provided object with the stored snapshot.
    /// </summary>
    /// <param name="snapshot">The object to compare with the stored snapshot</param>
    public static void ShouldMatchSnapshot(this object snapshot)
    {
        var snapper = SnapperFactory.CreateJsonSnapper(null);
        snapper.MatchSnapshot(snapshot, null).AssertSnapshot();
    }

    /// <summary>
    ///     Compares the provided object with the stored snapshot.
    /// </summary>
    /// <param name="snapshot">The object to compare with the stored snapshot</param>
    /// <param name="snapshotSettings">Settings to use for this snapshot</param>
    public static void ShouldMatchSnapshot(this object snapshot, SnapshotSettings snapshotSettings)
    {
        var snapper = SnapperFactory.CreateJsonSnapper(snapshotSettings);
        snapper.MatchSnapshot(snapshot, null).AssertSnapshot();
    }

    /// <summary>
    ///     Compares the provided object with the stored child snapshot.
    ///     Takes in a unique child name, best used in theory tests.
    /// </summary>
    /// <param name="snapshot">The object to compare with the stored child snapshot</param>
    /// <param name="childSnapshotName">The name of the child snapshot name. Must be unique per test.</param>
    public static void ShouldMatchChildSnapshot(this object snapshot, string childSnapshotName)
    {
        var snapper = SnapperFactory.CreateJsonSnapper(null);
        snapper.MatchSnapshot(snapshot, childSnapshotName).AssertSnapshot();
    }

    /// <summary>
    ///     Compares the provided object with the stored child snapshot.
    ///     Takes in a unique child name, best used in theory tests.
    /// </summary>
    /// <param name="snapshot">The object to compare with the stored child snapshot</param>
    /// <param name="childSnapshotName">The name of the child snapshot name. Must be unique per test.</param>
    /// <param name="snapshotSettings">Settings to use for this snapshot</param>
    public static void ShouldMatchChildSnapshot(this object snapshot, string childSnapshotName,
        SnapshotSettings snapshotSettings)
    {
        var snapper = SnapperFactory.CreateJsonSnapper(snapshotSettings);
        snapper.MatchSnapshot(snapshot, childSnapshotName).AssertSnapshot();
    }

    /// <summary>
    ///     Compares the provided object with an inline snapshot.
    ///     Use when snapshots are simple and small.
    /// </summary>
    /// <param name="snapshot">The object to compare with the inline snapshot</param>
    /// <param name="expectedSnapshot">The inline snapshot to which the object is compared with</param>
    public static void ShouldMatchInlineSnapshot(this object snapshot, object expectedSnapshot)
    {
        var snapper = SnapperFactory.GetJsonInlineSnapper(expectedSnapshot, null);
        snapper.MatchSnapshot(snapshot, null).AssertSnapshot();
    }

    /// <summary>
    ///     Compares the provided object with an inline snapshot.
    ///     Use when snapshots are simple and small.
    /// </summary>
    /// <param name="snapshot">The object to compare with the inline snapshot</param>
    /// <param name="expectedSnapshot">The inline snapshot to which the object is compared with</param>
    /// <param name="snapshotSettings">Settings to use for this snapshot</param>
    public static void ShouldMatchInlineSnapshot(this object snapshot, object expectedSnapshot,
        SnapshotSettings snapshotSettings)
    {
        var snapper = SnapperFactory.GetJsonInlineSnapper(expectedSnapshot, snapshotSettings);
        snapper.MatchSnapshot(snapshot, null).AssertSnapshot();
    }
}
