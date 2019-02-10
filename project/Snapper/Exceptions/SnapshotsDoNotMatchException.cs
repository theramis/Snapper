using System;
using Snapper.Core;
using Snapper.Json;

namespace Snapper.Exceptions
{
    internal class SnapshotsDoNotMatchException : Exception
    {
        public SnapshotsDoNotMatchException(SnapResult result)
            : base(CreateErrorMessage(result))
        {
        }

        private static string CreateErrorMessage(SnapResult result)
            => JsonDiffGenerator.GetDiffMessage(result.OldSnapshot, result.NewSnapshot);
    }
}