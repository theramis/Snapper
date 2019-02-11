using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Snapper.Core
{
    internal static class TestFrameworkHelper
    {
        private static readonly IList<string> _supportedTestFrameworksAttributes = new List<string>
        {
            {"Xunit.FactAttribute"},
            {"Xunit.TheoryAttribute"},
            {"NUnit.Framework.TestAttribute"},
            {"NUnit.Framework.TheoryAttribute"}
        };

        public static (MethodBase method, string filePath) GetCallingTestMethod()
        {
            var stackTrace = new StackTrace(3, true);
            foreach (var stackFrame in stackTrace.GetFrames() ?? new StackFrame[0])
            {
                var method = stackFrame.GetMethod();

                if (IsSnapperMethod(method))
                    continue;

                if (IsTestMethod(method))
                    return (method, stackFrame.GetFileName());

                var asyncMethod = GetMethodBaseOfAsyncMethod(method);
                if (IsTestMethod(asyncMethod))
                    return (asyncMethod, stackFrame.GetFileName());
            }

            // TODO Throw an error if nothing found
            // mention using [MethodImpl(MethodImplOptions.NoInlining)], or setting optimise code off
            // mention adding the framework into code
            // mention that it needs to be called from inside a test
            return (null, null);
        }

        private static bool IsSnapperMethod(MemberInfo method)
        {
            var methodAssembly = method?.ReflectedType?.Assembly.FullName;
            var snapperAssembly = Assembly.GetAssembly(typeof(SnapshotIdResolver)).FullName;
            return methodAssembly == snapperAssembly;
        }

        private static bool IsTestMethod(MemberInfo method)
        {
            var attribute = method?.CustomAttributes.FirstOrDefault(a =>
            {
                var attributeName = a.AttributeType.FullName;
                return _supportedTestFrameworksAttributes.Contains(attributeName);
            });

            return attribute != null;
        }

        private static MethodBase GetMethodBaseOfAsyncMethod(MemberInfo asyncMethod)
        {
            var generatedType = asyncMethod?.DeclaringType;
            var originalClass = generatedType?.DeclaringType;
            if (originalClass == null)
                return null;

            var matchingMethods =
                from methodInfo in originalClass.GetMethods()
                let attr = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>()
                where attr != null && attr.StateMachineType == generatedType
                select methodInfo;

            return matchingMethods.SingleOrDefault();
        }
    }
}
