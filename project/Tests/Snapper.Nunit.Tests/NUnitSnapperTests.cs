using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Snapper.Nunit.Tests
{
    public class NUnitSnapperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfStoredSnapshotIsMatching()
        {
            var actual = new JObject
            {
                {"TestProperty", "TestValue"}
            };
            Assert.That(actual, Is.EqualToSnapshot());
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
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
