namespace Snapper.Nunit;

// TODO add more extensions with settings
public class Matches
{
    public static EqualToSnapshotConstraint Snapshot()
    {
        return new EqualToSnapshotConstraint();
    }

    public static EqualToSnapshotConstraint ChildSnapshot(string snapshotName)
    {
        return new EqualToSnapshotConstraint(snapshotName);
    }
}
