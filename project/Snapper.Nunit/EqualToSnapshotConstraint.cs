using NUnit.Framework.Constraints;
using Snapper.Core;

namespace Snapper.Nunit;

// TODO add more extensions with settings
public class EqualToSnapshotConstraint : Constraint
{
    private readonly string? _childSnapshotName;
    private readonly SnapshotSettings? _snapshotSettings;

    public EqualToSnapshotConstraint(string? childSnapshotName = null, SnapshotSettings? snapshotSettings = null)
    {
        _childSnapshotName = childSnapshotName;
        _snapshotSettings = snapshotSettings;
    }

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        var snapResult = MatchSnapshot(actual);
        return new NUnitConstraintResult(this, actual, snapResult);
    }

    private SnapResult MatchSnapshot(object actual)
    {
        var snapper = SnapperFactory.CreateJsonSnapper(_snapshotSettings);
        return snapper.MatchSnapshot(actual, _childSnapshotName);
    }
}
