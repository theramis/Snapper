using System;
using Snapper.Core;

namespace Snapper.Exceptions
{
    internal class SnapshotsDoNotMatchException : Exception
    {
        public SnapshotsDoNotMatchException(SnapResult result)
            : base(Messages.GetSnapResultMessage(result))
        {
        }
    }
}