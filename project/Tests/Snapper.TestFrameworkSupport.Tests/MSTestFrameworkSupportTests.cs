using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Snapper.TestFrameworkSupport.Tests
{
    [TestClass]
    public class MSTestFrameworkSupportTests
    {
        [TestMethod]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void MSTestMethod()
        {
            "value".ShouldMatchInlineSnapshot("value");
        }

        [DataTestMethod]
        [DataRow("Data")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void MSTestDataTestMethod(string value)
        {
            value.ShouldMatchInlineSnapshot(value);
        }
    }
}
