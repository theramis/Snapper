using System;
using Newtonsoft.Json;
using Snapper.Core;

namespace Snapper.Exceptions
{
    internal class SnapshotDoesNotExistException : Exception
    {
        // TODO: Update to mention attribute and env var
        private const string ExceptionMessage = "A snapshot does not exist.\n\n";

        public SnapshotDoesNotExistException(SnapResult result)
            : base(ExceptionMessage)
        {
        }
    }
}
