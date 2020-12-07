using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Snapper.Tests
{
    public class SnapshotHelperTests
    {
        public class TestObject
        {
            public TestObject(int depth = 0)
            {
                if (depth > 0)
                {
                    NestedTestObject = new TestObject(depth - 1);
                }
                NestedTestObjects = Many(3, depth - 1).ToList();
            }

            public List<TestObject> NestedTestObjects { get; }
            public TestObject NestedTestObject { get; }

            public static IEnumerable<TestObject> Many(int count, int depth = 0) =>
                depth <= 0 ?
                new TestObject[0] :
                Enumerable.Range(0, count).Select(_ => new TestObject(depth)).ToArray();
        }

        [Fact]
        public void IgnoreRemovesProperty()
        {
            new TestObject(0).Ignore(x => x.NestedTestObject)
                .ShouldMatchInlineSnapshot(new { NestedTestObjects = new TestObject[0] });
        }

        [Fact]
        public void IgnoreRemovesNestedProperty()
        {
            new TestObject(2)
               .Ignore(x => x.NestedTestObject.NestedTestObject)
               .ShouldMatchInlineSnapshot(new
               {
                   NestedTestObject = new { NestedTestObjects = new TestObject[0] },
                   NestedTestObjects = TestObject.Many(3, 1)
               });
        }

        [Fact]
        public void IgnoreRemovesIndex()
        {
            new TestObject(1)
               .Ignore(
                   x => x.NestedTestObject,
                   x => x.NestedTestObjects[0])
               .ShouldMatchInlineSnapshot(
                new
                {
                    NestedTestObjects = TestObject.Many(2, 0)
                });
        }

        [Fact]
        public void IgnoreRemovesNestedIndex()
        {
            new TestObject(2)
               .Ignore(x => x.NestedTestObject.NestedTestObjects[0])
               .ShouldMatchInlineSnapshot(
                new
                {
                    NestedTestObjects = TestObject.Many(3, 1),
                    NestedTestObject = new
                    {
                        NestedTestObject = new TestObject(0),
                        NestedTestObjects = TestObject.Many(2, 0)
                    }
                });
        }

        [Fact]
        public void IgnoreThrowExceptionWithInvalidExpression()
        {
            Assert.Throws<InvalidOperationException>(() => new TestObject(2)
               .Ignore(x => x.NestedTestObjects[0].NestedTestObject));
        }
    }
}
