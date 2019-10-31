using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Snapper.TestFrameworkSupport.Tests
{
    public class NUnitFrameworkSupportTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void NUnitTestTestMethod()
        { 
            "value".ShouldMatchInlineSnapshot("value");
        }
        
        [Datapoint]
        public string value = string.Empty;
        
        [Theory]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void NUnitTheoryTestMethod(string value)
        { 
            value.ShouldMatchInlineSnapshot(value);
        }
    }
}
