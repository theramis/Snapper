namespace Snapper.Core
{
    public interface IAssert
    {
        void AssertEqual();
        void AssertNotEqual(string message);
        void AssertNotEqual(object oldValue, object newValue);
    }
}