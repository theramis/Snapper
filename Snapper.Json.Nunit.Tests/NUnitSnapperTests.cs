using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Snapper.Json.Nunit.Tests
{
    public class NUnitSnapperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestIfStoredSnapshotIsMatching()
        {
            var actual = new JObject
            {
                {"TestProperty", "TestValue"}
            };
            Assert.That(actual, Is.EqualToSnapshot());
        }

        [Test]
        public void TestIfNamedStoredSnapshotIsMatching()
        {
            var actual = new JObject
            {
                {"TestProperty2", "TestValue2"}
            };
            Assert.That(actual, Is.EqualToSnapshot("NamedSnapshot"));
        }
    }
}
