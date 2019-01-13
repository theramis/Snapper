using System;

namespace Snapper.Json.NUnit
{
    internal class NUnitAsserterException : Exception
    {
        public SnapResults Results { get; }

        public NUnitAsserterException(SnapResults results) => Results = results;
    }
}
