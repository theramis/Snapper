using Snapper.Json.Xunit;

namespace Xunit
{
    public partial class Assert
    {
        public static void MatchSnapshot(object value)
        {
            XUnitSnapper.Create().SnapObject(value);
        }

        public static void MatchSnapshot(string snapshotName, object value)
        {
            XUnitSnapper.Create().Snap(snapshotName, value);
        }
    }
}
