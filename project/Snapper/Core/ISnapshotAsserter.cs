namespace Snapper.Core
{
    internal interface ISnapshotAsserter
    {
        void AssertSnapshot(SnapResult snapResult);
    }
}