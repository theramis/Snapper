using Snapper.Core;
using Snapper.Json;
using Xunit;

namespace Snapper.Tests
{
    public class JsonSnapComparerTests
    {
        private readonly ISnapComparer _comparer;

        public JsonSnapComparerTests()
        {
            _comparer = new JsonSnapComparer();
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
            Assert.True(_comparer.Compare(obj1, obj2));
        }

        [Fact]
        public void Compare_DifferentPropertyOrder()
        {
            var obj1 = new { v = 1, vv = 2 };
            var obj2 = new { vv = 2, v = 1 };
            Assert.True(_comparer.Compare(obj1, obj2));
        }

        [Fact]
        public void Compare_ComplexObjects()
        {
            var obj1 = new
            {
                complex = new
                {
                    a = 1
                },
                v = 1
            };

            var obj2 = new
            {
                v = 1,
                complex = new
                {
                    a = 1
                }
            };
            Assert.True(_comparer.Compare(obj1, obj2));
        }

        [Fact]
        public void Compare_DifferentValues()
        {
            var obj1 = new { v = 1 };
            var obj2 = new { v = 2 };
            Assert.False(_comparer.Compare(obj1, obj2));
        }

        [Fact]
        public void Compare_DifferentValues_ComplexObjects()
        {
            var obj1 = new
            {
                complex = new
                {
                    a = 1
                },
                v = 1
            };

            var obj2 = new
            {
                v = 1,
                complex = new
                {
                    a = 2
                }
            };
            Assert.False(_comparer.Compare(obj1, obj2));
        }

        [Fact]
        public void Compare_DifferentValues_PropertyCase()
        {
            var obj1 = new
            {
                V = 1
            };

            var obj2 = new
            {
                v = 1
            };
            Assert.False(_comparer.Compare(obj1, obj2));
        }
    }
}
