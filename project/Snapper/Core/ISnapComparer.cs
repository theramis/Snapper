namespace Snapper.Core
{
    public interface ISnapComparer
    {
        bool Compare(object oldSnap, object newSnap);
    }
}