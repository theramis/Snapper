using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        [TestCase(1)]
        [TestCase(2)]
        public void TestWithCase(int a)
        {
            var actual = new JObject
            {
                {"TestProperty", a }
            };
            Assert.That(actual, Matches.ChildSnapshot(a.ToString()));
        }

        [TestCaseSource(nameof(TestCases))]
        public void TestWithCaseSource(string a)
        {
            var actual = new JObject
            {
                {"TestProperty", a }
            };
            Assert.That(actual, Matches.ChildSnapshot(a));
        }

        public static IEnumerable<TestCaseData> TestCases() {
            yield return new TestCaseData("A");
            yield return new TestCaseData("B");
        }
    }
}
