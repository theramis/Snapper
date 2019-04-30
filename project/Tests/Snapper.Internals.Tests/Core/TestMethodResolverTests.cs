using System.IO;
using FluentAssertions;
using Snapper.Core.TestMethodResolver;
using Xunit;

namespace Snapper.Internals.Tests.Core
{
    public class TestMethodResolverTests
    {
        private readonly ITestMethodResolver _testMethodResolver;

        public TestMethodResolverTests()
        {
            _testMethodResolver = new TestMethodResolver();
        }

        [Fact]
        public void XUnitFactTestMethod()
        {
            var testMethod = _testMethodResolver.ResolveTestMethod();

            var fileName = Path.Combine("Snapper.Internals.Tests", "Core",
                $"{nameof(TestMethodResolverTests)}.cs");

            testMethod.FileName.Should().EndWith(fileName);
            testMethod.MethodName.Should().Be(nameof(XUnitFactTestMethod));

            testMethod.BaseMethod.ReflectedType?.FullName.Should()
                .Be($"Snapper.Internals.Tests.Core.{nameof(TestMethodResolverTests)}");
            testMethod.BaseMethod.Name.Should().Be(nameof(XUnitFactTestMethod));
        }

        [Theory]
        [InlineData("Data")]
        public void XUnitTheoryTestMethod(string value)
        {
            var testMethod = _testMethodResolver.ResolveTestMethod();

            var fileName = Path.Combine("Snapper.Internals.Tests", "Core",
                $"{nameof(TestMethodResolverTests)}.cs");

            testMethod.FileName.Should().EndWith(fileName);
            testMethod.MethodName.Should().Be(nameof(XUnitTheoryTestMethod));

            testMethod.BaseMethod.ReflectedType?.FullName.Should()
                .Be($"Snapper.Internals.Tests.Core.{nameof(TestMethodResolverTests)}");
            testMethod.BaseMethod.Name.Should().Be(nameof(XUnitTheoryTestMethod));
        }
    }
}
