using System;
using Xunit;

namespace Snapper.Tests
{
    public class SnapperTests
    {
        [Fact]
        public void Test()
        {
            Environment.SetEnvironmentVariable("UpdateSnapshots", "false");
            var obj = new {value = 1};
            obj.ShouldMatchSnapshot();
        }
    }
}
