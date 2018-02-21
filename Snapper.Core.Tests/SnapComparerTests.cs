using Xunit;

namespace Snapper.Core.Tests
{
    public class SnapComparerTests
    {
        private readonly DefaultSnapComparer _comparer;

        public SnapComparerTests()
        {
            _comparer = new DefaultSnapComparer();
        }

        [Fact]
        public void Compare()
        {
            var obj = new { v = 1};
            Assert.True(_comparer.Compare(obj, obj));
        }
    }
}
