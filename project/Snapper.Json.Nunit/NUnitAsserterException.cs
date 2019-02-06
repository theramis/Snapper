using System;

namespace Snapper.Json.Nunit
{
    internal class NUnitAsserterException : Exception
    {
        public SnapResults Results { get; }

        public NUnitAsserterException(SnapResults results) => Results = results;
    }
}
