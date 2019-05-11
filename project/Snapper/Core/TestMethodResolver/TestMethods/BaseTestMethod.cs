using System.Linq;
using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal abstract class BaseTestMethod : ITestMethod
    {
        protected BaseTestMethod(MemberInfo method, string fileName)
        {
            FileName = fileName;
            BaseMethod = method;
        }

        protected abstract string AttributeName { get; }

        public bool IsTestMethod()
        {
            var attribute = BaseMethod?.CustomAttributes.FirstOrDefault(a =>
            {
                var attributeName = a.AttributeType.FullName;
                return attributeName == AttributeName;
            });

            return attribute != null;
        }

        public string MethodName => BaseMethod.Name;

        public string FileName { get; }

        public MemberInfo BaseMethod { get; }
    }
}