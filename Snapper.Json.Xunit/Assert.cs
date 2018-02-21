using Snapper.Json.Xunit;

// ReSharper disable once CheckNamespace
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
