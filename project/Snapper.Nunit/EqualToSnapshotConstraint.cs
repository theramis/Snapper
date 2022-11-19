using NUnit.Framework.Constraints;
using Snapper.Core;

namespace Snapper.Nunit;

// TODO add more extensions with settings
public class EqualToSnapshotConstraint : Constraint
{
    private readonly string _childSnapshotName;

    public EqualToSnapshotConstraint(string childSnapshotName = null)
    {
        _childSnapshotName = childSnapshotName;
    }

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        var snapResult = MatchSnapshot(actual);
        return new NUnitConstraintResult(this, actual, snapResult);
    }

    private SnapResult MatchSnapshot(object actual)
    {
        var snapper = SnapperFactory.CreateJsonSnapper(null);
        return snapper.MatchSnapshot(actual, _childSnapshotName);
    }
}
