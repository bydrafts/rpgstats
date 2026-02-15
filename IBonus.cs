namespace Drafts.Rpg
{
    public interface IBonus<out T>
    {
        public object Source { get; }
        public T Value { get; }
    }
}