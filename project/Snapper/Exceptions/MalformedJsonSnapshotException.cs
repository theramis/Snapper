using System;
using Snapper.Core;

namespace Snapper.Exceptions
{
    internal class MalformedJsonSnapshotException : Exception
    {
        public MalformedJsonSnapshotException(Exception exceptionParsingJson)
            : base(Messages.MalformedJsonSnapshotMessage, exceptionParsingJson)
        {
        }
    }
}
