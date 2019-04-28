using System.Runtime.CompilerServices;
using Xunit;

namespace Snapper.Tests
{
    public class SnapperInlineSnapshotTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void SnapshotsMatch_PassInObject()
        {
            var snapshot = new
            {
                TestValue = "value"
            };

            snapshot.ShouldMatchInlineSnapshot(new {
                TestValue = "value"
            });
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void SnapshotsMatch_PassInStringObject()
        {
            var snapshot = new
            {
                TestValue = "value"
            };

            snapshot.ShouldMatchInlineSnapshot(@"
            {
                'TestValue' : 'value'
            }");
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void SnapshotsMatch_PassInString()
        {
            const string snapshot = "Snapshot";

            snapshot.ShouldMatchInlineSnapshot("Snapshot");
        }
    }
}
