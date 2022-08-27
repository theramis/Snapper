using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using static NExpect.Expectations;
using Xunit;

namespace Snapper.NExpect.Tests
{
    public class NExpectSnapperTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfStoredSnapshotIsMatching()
        {
            var actual = new JObject
            {
                {"TestProperty", "TestValue"}
            };

            Expect(actual).To.MatchSnapshot();
        }
        
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfStoredSnapshotDoesNotMatch()
        {
            var actual = new JObject
            {
                {"TestProperty", "Match"}
            };

            Expect(actual).Not.To.MatchSnapshot();
            Expect(actual).To.Not.MatchSnapshot();
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfChildSnapshotIsMatching()
        {
            var actual = new JObject
            {
                {"TestProperty", "TestValue"}
            };
            
            Expect(actual).To.MatchChildSnapshot("ChildSnapshot");
        }
        
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfChildSnapshotDoesNotMatch()
        {
            var actual = new JObject
            {
                {"TestProperty", "Match"}
            };
            Expect(actual).Not.To.MatchChildSnapshot("ChildSnapshot");
            Expect(actual).To.Not.MatchChildSnapshot("ChildSnapshot");
        }
        
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfInlineSnapshotIsMatching()
        {
            var actual = new JObject
            {
                {"TestProperty", "TestValue"}
            };
            
            Expect(actual).To.MatchInlineSnapshot(new {
                TestProperty = "TestValue"
            });
        }
        
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestIfInlineSnapshotDoesNotMatch()
        {
            var actual = new JObject
            {
                {"TestProperty", "Match"}
            };
            
            Expect(actual).Not.To.MatchInlineSnapshot(new {
                TestProperty = "Not Match"
            });
            Expect(actual).To.Not.MatchInlineSnapshot(new {
                TestProperty = "Not Match"
            });
        }
    }
}
