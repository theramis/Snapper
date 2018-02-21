using Xunit;

namespace Snapper.Core.Tests
{
    public class StaticPathResolverTests
    {
        private readonly StaticPathResolver _resolver;
        private const string SnapshotDir = @"c:\dir";

        public StaticPathResolverTests()
        {
            _resolver = new StaticPathResolver(SnapshotDir);
        }

        [Fact]
        public void ResolvePath()
        {
            var path = _resolver.ResolvePath("snap_name");

            Assert.Equal($"{SnapshotDir}\\snap_name", path);
        }
    }
}
