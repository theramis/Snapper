using System;
using NUnit.Framework.Constraints;
using NUnitFramework = NUnit.Framework;

namespace Snapper.Json.Nunit
{
    public static class EqualToSnapshotConstraintExtensions
    {
        public static EqualToSnapshotConstraint EqualToSnapshot(this ConstraintExpression expression)
        {
            var constraint = new EqualToSnapshotConstraint();
            expression.Append(constraint);
            return constraint;
        }
    }

    public class Is : NUnitFramework.Is
    {
        public static EqualToSnapshotConstraint EqualToSnapshot(string snapshotName = null)
        {
            return new EqualToSnapshotConstraint(snapshotName);
        }
    }

    public class EqualToSnapshotConstraint : Constraint
    {
        private readonly string _snapshotName;

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var matchSnapshot = MatchSnapshot(actual);
            return new NUnitConstraintResult(this, actual, matchSnapshot);
        }

        public EqualToSnapshotConstraint(string snapshotName = null)
        {
            _snapshotName = snapshotName;
        }

        private SnapResults MatchSnapshot(object actual)
        {
            return _snapshotName == null
                ? NUnitSnapper.MatchSnapshot(actual)
                : NUnitSnapper.MatchSnapshot(_snapshotName, actual);
        }
    }

    internal class NUnitConstraintResult : ConstraintResult
    {
        private readonly SnapResults _snapResults;

        public NUnitConstraintResult(IConstraint constraint, object actualValue, ConstraintStatus status) : base(
            constraint, actualValue, status)
        {
            throw new NotSupportedException();
        }

        public NUnitConstraintResult(IConstraint constraint, object actualValue, bool isSuccess) : base(constraint,
            actualValue, isSuccess)
        {
            throw new NotSupportedException();
        }

        public NUnitConstraintResult(IConstraint constraint, object actualValue, SnapResults snapResults) : base(
            constraint, actualValue)
        {
            _snapResults = snapResults;
            Status = snapResults.Result == Result.ValueEqualToSnapshot
                ? ConstraintStatus.Success
                : ConstraintStatus.Failure;
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            writer.WriteValue(_snapResults.Differences);
        }
    }
}
