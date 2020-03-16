using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Snapper.Core.TestMethodResolver.TestMethods;
using Snapper.Exceptions;

namespace Snapper.Core.TestMethodResolver
{
    internal class TestMethodResolver : ITestMethodResolver
    {
        private static IEnumerable<ITestMethod> GetTestMethods(MemberInfo method, string fileName)
            => new List<ITestMethod>
            {
                new XunitFactMethod(method, fileName),
                new XunitTheoryMethod(method, fileName),
                new NunitTestMethod(method, fileName),
                new NunitTheoryMethod(method, fileName),
                new MSTestTestMethod(method, fileName),
                new MSTestDataTestMethod(method, fileName),
            };

        public ITestMethod ResolveTestMethod()
        {
            try
            {
                var stackTrace = new StackTrace(1, true);
                foreach (var stackFrame in stackTrace.GetFrames() ?? new StackFrame[0])
                {
                    var method = stackFrame.GetMethod();

                    if (IsAsyncMethod(method))
                        method = GetMethodBaseOfAsyncMethod(method);

                    if (IsSnapperMethod(method))
                        continue;

                    if (TryGetTestMethod(method, stackFrame.GetFileName(), out var testMethod))
                        return testMethod;
                }
            }
            catch (UnableToDetermineTestFilePathException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SupportedTestMethodNotFoundException(ex);
            }

            throw new SupportedTestMethodNotFoundException();
        }

        private static bool IsSnapperMethod(MemberInfo method)
        {
            var methodAssembly = method?.DeclaringType?.Assembly.FullName;
            var snapperAssembly = Assembly.GetAssembly(typeof(TestMethodResolver)).FullName;
            return methodAssembly == snapperAssembly;
        }

        private static bool IsAsyncMethod(MemberInfo method)
            => typeof(IAsyncStateMachine).IsAssignableFrom(method.DeclaringType);

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

        private static bool TryGetTestMethod(MemberInfo method, string fileName, out ITestMethod testMethod)
        {
            foreach (var tm in GetTestMethods(method, fileName))
            {
                if (tm.IsTestMethod())
                {
                    testMethod = tm;

                    if (fileName == null)
                    {
                        throw new UnableToDetermineTestFilePathException();
                    }
                    return true;
                }
            }

            testMethod = null;
            return false;
        }
    }
}
