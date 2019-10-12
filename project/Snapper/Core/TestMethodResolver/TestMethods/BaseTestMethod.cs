using System;
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
            try
            {
                var attribute = BaseMethod?.CustomAttributes.FirstOrDefault(a =>
                {
                    var type = a.AttributeType;
                    do
                    {
                        if (type.FullName == AttributeName)
                            return true;

                        type = type.BaseType;
                    } while (type != null);

                    return false;
                });

                return attribute != null;
            }
            catch (NotImplementedException)
            {
                // MemberInfo.CustomAttributes in some cases can throw a NotImplementedException.
                // In this case we assume that the method is not a test method.
                // See issue: https://github.com/theramis/Snapper/issues/27
                return false;
            }
        }

        public string MethodName => BaseMethod.Name;

        public string FileName { get; }

        public MemberInfo BaseMethod { get; }
    }
}
