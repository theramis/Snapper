using FluentAssertions;
using Snapper.Core;
using Snapper.Json;
using Xunit;

namespace Snapper.Internals.Tests.Json
{
    public class JsonSnapshotComparerTests
    {
        private readonly ISnapshotComparer _comparer;

        public JsonSnapshotComparerTests()
        {
            _comparer = new JsonSnapshotComparer();
        }

        [Fact]
        public void Compare_SameObject()
        {
            var obj = new { v = 1 };
            _comparer.CompareSnapshots(obj, obj).Should().BeTrue();
        }

        [Fact]
        public void Compare_SameObject_DifferentInstances()
        {
            var obj1 = new { v = 1 };
            var obj2 = new { v = 1 };
            _comparer.CompareSnapshots(obj1, obj2).Should().BeTrue();
        }

        [Fact]
        public void Compare_DifferentPropertyOrder()
        {
            var obj1 = new { v = 1, vv = 2 };
            var obj2 = new { vv = 2, v = 1 };
            _comparer.CompareSnapshots(obj1, obj2).Should().BeTrue();
        }

        [Fact]
        public void Compare_ComplexObjects()
        {
            var obj1 = new
            {
                v = 1,
                complex = new
                {
                    a = 1
                }
            };

            var obj2 = new
            {
                v = 1,
                complex = new
                {
                    a = 1
                }
            };
            _comparer.CompareSnapshots(obj1, obj2).Should().BeTrue();
        }

        [Fact]
        public void Compare_DifferentValues()
        {
            var obj1 = new { v = 1 };
            var obj2 = new { v = 2 };
            _comparer.CompareSnapshots(obj1, obj2).Should().BeFalse();
        }

        [Fact]
        public void Compare_DifferentValues_ComplexObjects()
        {
            var obj1 = new
            {
                v = 1,
                complex = new
                {
                    a = 1
                }
            };

            var obj2 = new
            {
                v = 1,
                complex = new
                {
                    a = 2
                }
            };
            _comparer.CompareSnapshots(obj1, obj2).Should().BeFalse();
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
            _comparer.CompareSnapshots(obj1, obj2).Should().BeFalse();
        }
    }
}
