using System;
using NUnit.Framework.Constraints;
using Snapper.Core;

namespace Snapper.Nunit
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

    public class Is : NUnit.Framework.Is
    {
        public static EqualToSnapshotConstraint EqualToSnapshot(string snapshotName = null)
        {
            return new EqualToSnapshotConstraint(snapshotName);
        }
    }

    public class EqualToSnapshotConstraint : Constraint
    {
        private readonly string _snapshotName;

        public EqualToSnapshotConstraint(string snapshotName = null)
        {
            _snapshotName = snapshotName;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var matchSnapshot = MatchSnapshot(actual);
            return new NUnitConstraintResult(this, actual, matchSnapshot);
        }

        private SnapResult MatchSnapshot(object actual)
        {
            var snapper = NUnitSnapperFactory.GetNUnitSnapper();
            return _snapshotName == null
                   ? snapper.MatchSnapshot(actual)
                   : snapper.MatchSnapshot(actual, _snapshotName);
        }
    }

    internal class NUnitConstraintResult : ConstraintResult
    {
        private readonly SnapResult _snapResult;

        public NUnitConstraintResult(IConstraint constraint, object actualValue, SnapResult snapResult)
            : base(constraint, actualValue)
        {
            _snapResult = snapResult;
            if (snapResult.Status == SnapResultStatus.SnapshotsMatch ||
                snapResult.Status == SnapResultStatus.SnapshotUpdated)
            {
                Status = ConstraintStatus.Success;
            }
            else
            {
                Status = ConstraintStatus.Failure;
            }
        }

        public NUnitConstraintResult(IConstraint constraint, object actualValue, ConstraintStatus status)
            : base(constraint, actualValue, status)
        {
            throw new NotSupportedException();
        }

        public NUnitConstraintResult(IConstraint constraint, object actualValue, bool isSuccess)
            : base(constraint, actualValue, isSuccess)
        {
            throw new NotSupportedException();
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            writer.WriteValue(Messages.GetSnapResultMessage(_snapResult));
        }
    }
}
