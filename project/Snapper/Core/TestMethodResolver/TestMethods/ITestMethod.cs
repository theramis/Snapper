using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal interface ITestMethod
    {
        bool IsTestMethod();

        string InstanceName { get; }

        string FileName { get; }

        MemberInfo BaseMethod { get; }
    }
}