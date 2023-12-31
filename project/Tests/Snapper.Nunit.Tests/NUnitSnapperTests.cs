using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;

namespace Snapper.Nunit.Tests
{
    public class NUnitSnapperTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfStoredSnapshotIsMatching()
        {
            var actual = JsonSerializer.SerializeToElement(
                new Dictionary<string, string> { { "TestProperty", "TestValue" } }
            );
            Assert.That(actual, Matches.Snapshot());
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfNamedStoredSnapshotIsMatching()
        {
            var actual = JsonSerializer.SerializeToElement(
                new Dictionary<string, string> { { "TestProperty2", "TestValue2" } }
            );
            Assert.That(actual, Matches.ChildSnapshot("ChildSnapshot"));
        }

        [TestCase("TestProperty1", "TestValue1")]
        [TestCase("TestProperty2", "TestValue2")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfNamedStoredSnapshotIsMatchingTestCase(string property, string value)
        {
            var actual = JsonSerializer.SerializeToElement(new Dictionary<string, string> { { property, value } });
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
            var actual = JsonSerializer.SerializeToElement(new Dictionary<string, string> { { "TestProperty", value } });
            Assert.That(actual, Matches.ChildSnapshot($"ChildSnapshotFor{value}"));
        }
    }
}
