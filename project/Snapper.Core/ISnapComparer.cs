namespace Snapper.Core
{
    public interface ISnapComparer
    {
        bool Compare(object oldSnap, object newSnap);
    }

    public class DefaultSnapComparer : ISnapComparer
    {
        public bool Compare(object oldSnap, object newSnap)
            => oldSnap == newSnap;
    }
}