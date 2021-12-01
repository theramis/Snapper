using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class NunitTestCaseMethod : BaseTestMethod
    {
        public NunitTestCaseMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        { }

        protected override string AttributeName => "NUnit.Framework.TestCaseAttribute";
    }
}