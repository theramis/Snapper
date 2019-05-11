using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class XunitTheoryMethod : BaseTestMethod
    {
        public XunitTheoryMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        {}

        protected override string AttributeName => "Xunit.TheoryAttribute";
    }
}