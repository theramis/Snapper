using Newtonsoft.Json.Linq;
using Snapper.Core;

namespace Snapper.Json.Nunit
{
    internal class NUnitAsserter : IAssert
    {
        public void AssertEqual() => throw new NUnitAsserterException(SnapResults.ValueEqualToSnapshot());

        public void AssertNotEqual(string message) => throw new NUnitAsserterException(SnapResults.NoSnapshotPresent());

        public void AssertNotEqual(object oldValue, object newValue)
        {
            var old = JToken.FromObject(oldValue);
            var @new = JToken.FromObject(newValue);
            throw new NUnitAsserterException(
                SnapResults.ValueNotEqualToSnapshot(JsonDiff.GetDiffMessage(old, @new)));
        }
    }
}
