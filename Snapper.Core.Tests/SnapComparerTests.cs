using Xunit;

namespace Snapper.Core.Tests
{
    public class SnapComparerTests
    {
        private readonly ISnapComparer _comparer;

        public SnapComparerTests()
        {
            _comparer = new DefaultSnapComparer();
        }

        [Fact]
        public void Compare_SameObject()
        {
            var obj = new { v = 1 };
            Assert.True(_comparer.Compare(obj, obj));
        }
        
        [Fact]
        public void Compare_SameObject_DifferentInstances()
        {
            var obj1 = new { v = 1 };
            var obj2 = new { v = 1 };
            Assert.False(_comparer.Compare(obj1, obj2));
        }
        
        [Fact]
        public void Compare_DifferentPropertyOrder()
        {
            var obj1 = new { v = 1, vv = 2 };
            var obj2 = new { vv = 2, v = 1 };
            Assert.False(_comparer.Compare(obj1, obj2));
        }
        
        [Fact]
        public void Compare_DifferentValues()
        {
            var obj1 = new { v = 1 };
            var obj2 = new { v = 2 };
            Assert.False(_comparer.Compare(obj1, obj2));
        }
    }
}
