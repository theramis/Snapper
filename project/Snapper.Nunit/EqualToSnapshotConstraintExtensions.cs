using NUnit.Framework.Constraints;

namespace Snapper.Nunit;

public static class EqualToSnapshotConstraintExtensions
{
    public static EqualToSnapshotConstraint EqualToSnapshot(this ConstraintExpression expression)
    {
        var constraint = new EqualToSnapshotConstraint();
        expression.Append(constraint);
        return constraint;
    }
}
