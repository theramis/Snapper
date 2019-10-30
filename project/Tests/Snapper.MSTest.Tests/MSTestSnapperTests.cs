using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snapper.Attributes;

namespace Snapper.MSTest.Tests
{
    [TestClass]
    public class MSTestSnapperTests
    {
        [TestMethod]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfStoredSnapshotIsMatching()
        {
            var actual = new { TestProperty = "TestValue" };
            actual.ShouldMatchSnapshot();
        }

        [TestMethod]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfNamedStoredSnapshotIsMatching()
        {
            var actual = new { TestProperty2 = "TestValue2" };
            actual.ShouldMatchChildSnapshot("ChildSnapshot");
        }
    }
}
