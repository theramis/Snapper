using System.Reflection;

namespace Snapper.Core.TestMethodResolver.TestMethods
{
    internal class NunitTheoryMethod : BaseTestMethod
    {
        public NunitTheoryMethod(MemberInfo method, string fileName)
            : base(method, fileName)
        {}

        protected override string AttributeName => "NUnit.Framework.TheoryAttribute";
    }
}