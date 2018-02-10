using Snapper.Json.Xunit;

namespace Xunit
{
    public partial class Assert
    {
        public static void MatchSnapshot(object value)
        {
            XUnitSnapper.Create().SnapObject(value);
        }
    }
}
