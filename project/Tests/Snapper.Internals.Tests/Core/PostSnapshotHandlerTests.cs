using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Moq;
using Snapper.Attributes;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;
using Snapper.Core.TestMethodResolver.TestMethods;
using Xunit;

namespace Snapper.Internals.Tests.Core
{
    public class PostSnapshotHandlerTests
    {
        public static bool HandleMethodCalled { get; set; }
        
        private readonly Mock<ISnapshotHandler> _handler;
        private readonly Mock<ITestMethodResolver> _testMethodResolver;
        private readonly SnapshotId _snapshotId;

        public PostSnapshotHandlerTests()
        {
            _handler = new Mock<ISnapshotHandler>();
            _testMethodResolver = new Mock<ITestMethodResolver>();
            _snapshotId = new SnapshotId("", "", "");
            HandleMethodCalled = false;
        }

        [Fact]
        public void Snap_calls_PostSnapshotAttributes()
        {
            _testMethodResolver
                .Setup(_ => _.ResolveTestMethod())
                .Returns(new ProxyTestMethod());
            var sut = new PostSnapshotHandler(_handler.Object, _testMethodResolver.Object);
            sut.Snap(_snapshotId, new { v = 1 });
            HandleMethodCalled.Should().BeTrue();
        }

        private class ProxyAttribute : PostSnapshotAttribute
        {
            public override void Handle(SnapResult result)
            {
                HandleMethodCalled = true;
            }
        }

        private class ProxyTestMethod : ITestMethod
        {
            public ProxyTestMethod()
            {
                BaseMethod = GetType().GetMethod(nameof(TestMethod));
            }

            public string MethodName => BaseMethod.Name;

            public string FileName => "";

            public MemberInfo BaseMethod { get; }

            public bool IsTestMethod() => true;

            [Proxy]
            public void TestMethod() { }
        }
    }
}
