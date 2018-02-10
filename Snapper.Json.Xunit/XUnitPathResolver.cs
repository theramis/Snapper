using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Snapper.Core;
using Xunit;

namespace Snapper.Json.Xunit
{
    public class XUnitPathResolver : IPathResolver
    {
        public string ResolvePath(string snapshotName)
        {
            var (methodName, filePath) = GetCallingTestInfo();
            var directory = Path.GetDirectoryName(filePath);
            var snapName = string.IsNullOrWhiteSpace(snapshotName) ? methodName : snapshotName;

            return Path.Combine(directory, "_snapshots", $"{snapName}.json");
        }

        private static (string methodName, string filePath) GetCallingTestInfo()
        {
            var stackTrace = new StackTrace(2, true);
            foreach (var stackFrame in stackTrace.GetFrames() ?? new StackFrame[0])
            {
                var method = stackFrame.GetMethod();

                if (IsXUnitTestMethod(method))
                    return (method.Name, stackFrame.GetFileName());

                var asyncMethod = GetMethodBaseOfAsyncMethod(method);
                if (IsXUnitTestMethod(asyncMethod))
                    return (asyncMethod.Name, stackFrame.GetFileName());
            }

            throw new InvalidOperationException("Snapshots can only be created from classes decorated with the [Fact] or [Theory] attribute");
        }

        private static bool IsXUnitTestMethod(ICustomAttributeProvider method)
            => method?.GetCustomAttributes(typeof(FactAttribute), true).Any() ?? false;

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