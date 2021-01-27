using System.Linq;
using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class NunitTestCaseMethod : BaseTestMethod, ITestMethod
    {
        public NunitTestCaseMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        { }

        protected override string AttributeName => "NUnit.Framework.TestCaseAttribute";
    }
}