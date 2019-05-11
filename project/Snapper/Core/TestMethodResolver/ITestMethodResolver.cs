using Snapper.Core.TestMethodResolver.TestMethods;

namespace Snapper.Core.TestMethodResolver
{
    internal interface ITestMethodResolver
    {
        ITestMethod ResolveTestMethod();
    }
}