namespace Snapper.Core
{
    public interface ISnapStore
    {
        object GetSnap(string snapId);

        void StoreSnap(string snapId, object snap);
    }
}
