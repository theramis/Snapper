using System.Diagnostics.CodeAnalysis;
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
        public void ResolvedTestMethod_HasCorrectData()
        {
            var testMethod = _testMethodResolver.ResolveTestMethod();

            var fileName = Path.Combine("Snapper.Internals.Tests", "Core",
                $"{nameof(TestMethodResolverTests)}.cs");

            testMethod.FileName.Should().EndWith(fileName);
            testMethod.MethodName.Should().Be(nameof(ResolvedTestMethod_HasCorrectData));

            testMethod.BaseMethod.ReflectedType?.FullName.Should()
                .Be($"Snapper.Internals.Tests.Core.{nameof(TestMethodResolverTests)}");
            testMethod.BaseMethod.Name.Should().Be(nameof(ResolvedTestMethod_HasCorrectData));
        }
    }
}
