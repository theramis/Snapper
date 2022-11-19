using System;
using NUnit.Framework.Constraints;
using Snapper.Core;

namespace Snapper.Nunit;

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
