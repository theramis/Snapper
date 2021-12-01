using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class NunitTestCaseSourceMethod : BaseTestMethod
    {
        public NunitTestCaseSourceMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        { }

        protected override string AttributeName => "NUnit.Framework.TestCaseSourceAttribute";
    }
}