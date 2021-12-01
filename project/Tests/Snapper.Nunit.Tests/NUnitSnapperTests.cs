using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Snapper.Nunit.Tests
{
    public class NUnitSnapperTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfStoredSnapshotIsMatching()
        {
            var actual = new JObject
            {
                {"TestProperty", "TestValue"}
            };
            Assert.That(actual, Matches.Snapshot());
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfNamedStoredSnapshotIsMatching()
        {
            var actual = new JObject
            {
                {"TestProperty2", "TestValue2"}
            };
            Assert.That(actual, Matches.ChildSnapshot("ChildSnapshot"));
        }

        [TestCase("TestProperty1", "TestValue1")]
        [TestCase("TestProperty2", "TestValue2")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfNamedStoredSnapshotIsMatchingTestCase(string property, string value)
        {
            var actual = new JObject
            {
                {property, value}
            };
            Assert.That(actual, Matches.ChildSnapshot($"ChildSnapshotFor{value}"));
        }

        private static List<string> ExampleTestCaseSource = new List<string>
        {
            "TestValue1",
            "TestValue2",
        };

        [TestCaseSource(nameof(ExampleTestCaseSource))]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfNamedStoredSnapshotIsMatchingTestCaseSource(string value)
        {
            var actual = new JObject
            {
                {"TestProperty", value}
            };
            Assert.That(actual, Matches.ChildSnapshot($"ChildSnapshotFor{value}"));
        }
    }
}
