using Snapper.Attributes;
using Xunit;

namespace Snapper.Tests
{
    [StoreSnapshotsPerClass]
    public class SnapperTests
    {
        [Fact]
        public void Test()
        {
            var obj = new { value = 1 };
            obj.ShouldMatchSnapshot();
        }

        [Fact]
        public void Test2()
        {
            var obj = new { value = 5 };
            obj.ShouldMatchSnapshot();
        }
    }
}
