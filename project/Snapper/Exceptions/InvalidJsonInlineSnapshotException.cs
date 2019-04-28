using System;
using Snapper.Core;

namespace Snapper.Exceptions
{
    internal class InvalidJsonInlineSnapshotException : Exception
    {
        public InvalidJsonInlineSnapshotException(Exception exceptionParsingJson)
            : base(Messages.InvalidJsonInlineSnapshotMessage, exceptionParsingJson)
        {
        }
    }
}
