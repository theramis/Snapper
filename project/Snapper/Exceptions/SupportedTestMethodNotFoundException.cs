using System;
using Snapper.Core;

namespace Snapper.Exceptions
{
    internal class SupportedTestMethodNotFoundException : Exception
    {
        public SupportedTestMethodNotFoundException()
            : base(Messages.TestMethodNotFoundMessage)
        {
        }

        public SupportedTestMethodNotFoundException(Exception baseException)
            : base(Messages.TestMethodNotFoundMessage, baseException)
        {
        }
    }
}
