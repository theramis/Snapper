using System;
using Snapper.Core;

namespace Snapper.Exceptions
{
    internal class SnapshotDoesNotExistException : Exception
    {
        public SnapshotDoesNotExistException(SnapResult result)
            : base(Messages.GetSnapResultMessage(result))
        {
        }
    }
}
