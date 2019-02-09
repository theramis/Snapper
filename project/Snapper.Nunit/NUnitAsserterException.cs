using System;

namespace Snapper.Nunit
{
    internal class NUnitAsserterException : Exception
    {
        public SnapResults Results { get; }

        public NUnitAsserterException(SnapResults results) => Results = results;
    }
}
