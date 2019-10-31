using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class MSTestDataTestMethod : BaseTestMethod
    {
        public MSTestDataTestMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        { }

        protected override string AttributeName => "Microsoft.VisualStudio.TestTools.UnitTesting.DataTestMethodAttribute";
    }
}
