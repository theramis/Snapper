using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class NunitTestMethod : BaseTestMethod
    {
        public NunitTestMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        {}

        protected override string AttributeName => "NUnit.Framework.TestAttribute";
    }
}