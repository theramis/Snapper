using System.Runtime.CompilerServices;
using Xunit;

namespace Snapper.TestFrameworkSupport.Tests
{
    public class XUnitFrameworkSupportTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void XUnitFactTestMethod()
        { 
            "value".ShouldMatchInlineSnapshot("value");
        }
        
        [Theory]
        [InlineData("Data")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void XUnitTheoryTestMethod(string value)
        { 
            value.ShouldMatchInlineSnapshot(value);
        }
    }
}
