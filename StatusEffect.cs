namespace Drafts.Rpg
{
    public interface IStatusEffect<T>
    {
        object Key { get; }
        void Apply(T ctx);
        void Remove(T ctx);
        void Stack(T ctx, IStatusEffect<T> other);
        /// <summary>Return false to remove.</summary>
        bool Tick(T ctx, float deltaTime);
    }
}