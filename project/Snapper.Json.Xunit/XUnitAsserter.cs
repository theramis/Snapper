using Newtonsoft.Json.Linq;
using Snapper.Core;
using Xunit;

namespace Snapper.Json.Xunit
{
    public class XUnitAsserter : IAssert
    {
        public void AssertEqual()
        {
            Assert.True(true);
        }

        public void AssertNotEqual(string message)
        {
            Assert.True(false, message);
        }

        public void AssertNotEqual(object oldValue, object newValue)
        {
            var old = JToken.FromObject(oldValue);
            var @new = JToken.FromObject(newValue);
            var diff = JsonDiff.GetDiffMessage(old, @new);
            Assert.True(false, $"Received value does not match stored snapshot.\n {diff}");
        }
    }
}
