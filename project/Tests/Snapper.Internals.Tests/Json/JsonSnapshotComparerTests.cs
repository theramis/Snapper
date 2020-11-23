using System;
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
            var result = _comparer.CompareSnapshots(obj, obj);
            result.Status.Should().Be(SnapResultStatus.SnapshotsMatch);
        }

        [Fact]
        public void Compare_SameObject_DifferentInstances()
        {
            var obj1 = new { v = 1 };
            var obj2 = new { v = 1 };
            var result = _comparer.CompareSnapshots(obj1, obj2);
            result.Status.Should().Be(SnapResultStatus.SnapshotsMatch);
        }

        [Fact]
        public void Compare_DifferentPropertyOrder()
        {
            var obj1 = new { v = 1, vv = 2 };
            var obj2 = new { vv = 2, v = 1 };
            var result = _comparer.CompareSnapshots(obj1, obj2);
            result.Status.Should().Be(SnapResultStatus.SnapshotsMatch);
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
            var result = _comparer.CompareSnapshots(obj1, obj2);
            result.Status.Should().Be(SnapResultStatus.SnapshotsMatch);
        }

        [Fact]
        public void Compare_DifferentValues()
        {
            var obj1 = new { v = 1 };
            var obj2 = new { v = 2 };
            var result = _comparer.CompareSnapshots(obj1, obj2);
            result.Status.Should().Be(SnapResultStatus.SnapshotsDoNotMatch);
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
            var result = _comparer.CompareSnapshots(obj1, obj2);
            result.Status.Should().Be(SnapResultStatus.SnapshotsDoNotMatch);
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
            var result = _comparer.CompareSnapshots(obj1, obj2);
            result.Status.Should().Be(SnapResultStatus.SnapshotsDoNotMatch);
        }

        [Fact]
        public void Compare_Null()
        {
            var obj = new { v = 1 };
            var result = _comparer.CompareSnapshots(null, obj);
            result.Status.Should().Be(SnapResultStatus.SnapshotDoesNotExist);
        }

        [Fact]
        public void Compare_fails_when_newSpapshot_is_null()
        {
            var obj = new { v = 1 };
            Action test = () => _comparer.CompareSnapshots(obj, null);
            test.Should().Throw<ArgumentNullException>();
        }
    }
}
