using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class MSTestTestMethod : BaseTestMethod
    {
        public MSTestTestMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        { }

        protected override string AttributeName => "Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute";
    }
}
