namespace Snapper.Core
{
    internal interface ISnapshotSanitiser
    {
        object SanitiseSnapshot(object snapshot);
    }
}