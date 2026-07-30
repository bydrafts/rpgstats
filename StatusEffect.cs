namespace Drafts.Rpg
{
    public interface IStatusEffect<in T>
    {
        void Apply(T ctx);
        void Stack(T ctx, T other);
        void Remove(T ctx);
        /// <summary>Return false to remove.</summary>
        bool Tick(T ctx, float deltaTime);
    }
}