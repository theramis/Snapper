using System;
using Snapper.Core;

namespace Snapper.Exceptions
{
    internal class UnableToDetermineTestFilePathException : Exception
    {
        public UnableToDetermineTestFilePathException()
            : base(Messages.UnableToDetermineTestFilePathMessage)
        {
        }
    }
}
