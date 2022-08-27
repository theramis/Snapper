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

    [Obsolete("Snapper.Nunit.Is will be removed in a future release.  Please use Snapper.Nunit.Matches")]
    public class Is : NUnit.Framework.Is
    {
        public static EqualToSnapshotConstraint EqualToSnapshot()
        {
            return new EqualToSnapshotConstraint();
        }

        public static EqualToSnapshotConstraint EqualToSnapshot(SnapshotId snapshotId)
        {
            return new EqualToSnapshotConstraint(snapshotId);
        }

        public static EqualToSnapshotConstraint EqualToChildSnapshot(string childSnapshotName)
        {
            return new EqualToSnapshotConstraint(childSnapshotName);
        }
    }

    public class Matches
    {
        public static EqualToSnapshotConstraint Snapshot()
        {
            return new EqualToSnapshotConstraint();
        }

        public static EqualToSnapshotConstraint Snapshot(SnapshotId snapshotId)
        {
            return new EqualToSnapshotConstraint(snapshotId);
        }

        public static EqualToSnapshotConstraint ChildSnapshot(string snapshotName)
        {
            return new EqualToSnapshotConstraint(snapshotName);
        }
    }

    public class EqualToSnapshotConstraint : Constraint
    {
        private readonly SnapshotId _snapshotId;
        private readonly string _childSnapshotName;

        public EqualToSnapshotConstraint(string childSnapshotName = null)
        {
            _childSnapshotName = childSnapshotName;
        }

        public EqualToSnapshotConstraint(SnapshotId snapshotId)
        {
            _snapshotId = snapshotId;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            SnapResult snapResult;
            if (_snapshotId != null)
            {
                var snapper = SnapperFactory.GetJsonSnapper();
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
            var snapper = SnapperFactory.GetJsonSnapper();
            return _childSnapshotName == null
                   ? snapper.MatchSnapshot(actual)
                   : snapper.MatchSnapshot(actual, _childSnapshotName);
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
