using NUnit.Framework.Constraints;
using Snapper.Core;

namespace Snapper.Nunit;

// TODO add more extensions with settings
public class EqualToSnapshotConstraint : Constraint
{
    private readonly SnapshotId _snapshotId;
    private readonly string _childSnapshotName;

    public EqualToSnapshotConstraint(string childSnapshotName = null)
    {
        _childSnapshotName = childSnapshotName;
    }

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        SnapResult snapResult;
        if (_snapshotId != null)
        {
            var snapper = SnapperFactory.CreateJsonSnapper(null);
            snapResult = snapper.MatchSnapshot(actual, _snapshotId);
        }
        else
        {
            snapResult = MatchSnapshot(actual);
        }

        return new NUnitConstraintResult(this, actual, snapResult);
    }

    private SnapResult MatchSnapshot(object actual)
    {
        var snapper = SnapperFactory.CreateJsonSnapper(null);
        return _childSnapshotName == null
            ? snapper.MatchSnapshot(actual)
            : snapper.MatchSnapshot(actual, _childSnapshotName);
    }
}
