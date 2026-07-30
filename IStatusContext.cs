namespace Drafts.Rpg
{
    public interface IStatusContext<in T>
    {
        object Key { get; }
        IStatusEffect<T> Effect { get; }
    }
}