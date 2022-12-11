namespace Snapper.Nunit;

public class Matches
{
    /// <summary>
    ///     Compares the provided object with the stored snapshot.
    /// </summary>
    public static EqualToSnapshotConstraint Snapshot()
    {
        return new EqualToSnapshotConstraint();
    }

    /// <summary>
    ///     Compares the provided object with the stored snapshot.
    /// </summary>
    /// <param name="snapshotSettings">Settings to use for this snapshot</param>
    public static EqualToSnapshotConstraint Snapshot(SnapshotSettings snapshotSettings)
    {
        return new EqualToSnapshotConstraint(null, snapshotSettings);
    }

    /// <summary>
    ///     Compares the provided object with the stored child snapshot.
    ///     Takes in a unique child name, best used in theory tests.
    /// </summary>
    /// <param name="childSnapshotName">The name of the child snapshot name. Must be unique per test.</param>
    public static EqualToSnapshotConstraint ChildSnapshot(string childSnapshotName)
    {
        return new EqualToSnapshotConstraint(childSnapshotName);
    }

    /// <summary>
    ///     Compares the provided object with the stored child snapshot.
    ///     Takes in a unique child name, best used in theory tests.
    /// </summary>
    /// <param name="childSnapshotName">The name of the child snapshot name. Must be unique per test.</param>
    /// <param name="snapshotSettings">Settings to use for this snapshot</param>
    public static EqualToSnapshotConstraint ChildSnapshot(string childSnapshotName, SnapshotSettings snapshotSettings)
    {
        return new EqualToSnapshotConstraint(childSnapshotName, snapshotSettings);
    }
}
