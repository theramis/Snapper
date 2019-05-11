using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal interface ITestMethod
    {
        bool IsTestMethod();

        string MethodName { get; }

        string FileName { get; }

        MemberInfo BaseMethod { get; }
    }
}