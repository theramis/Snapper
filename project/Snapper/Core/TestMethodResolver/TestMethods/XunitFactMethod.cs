using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class XunitFactMethod : BaseTestMethod
    {
        public XunitFactMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        {}

        protected override string AttributeName => "Xunit.FactAttribute";
    }
}