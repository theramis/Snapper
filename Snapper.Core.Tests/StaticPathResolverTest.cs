using Xunit;

namespace Snapper.Core.Tests
{
    public class StaticPathResolverTest
    {
        [Fact]
        public void ResolvePath()
        {
            const string dir = @"c:\dir";
            var resolver = new StaticPathResolver(dir);

            var path = resolver.ResolvePath("snap_name");

            Assert.Equal($"{dir}\\snap_name", path);
        }
    }
}
