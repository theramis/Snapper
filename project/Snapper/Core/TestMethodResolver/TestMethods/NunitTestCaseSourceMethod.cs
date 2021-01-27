using System.Linq;
using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class NunitTestCaseSourceMethod : BaseTestMethod, ITestMethod
    {
        public NunitTestCaseSourceMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        { }

        protected override string AttributeName => "NUnit.Framework.TestCaseSourceAttribute";
    }
}