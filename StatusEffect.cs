namespace Drafts.Rpg
{
    public interface IStatusEffect<T>
    {
        T Context { get; }
        IStatusEffect<T> Clone(T ctx);
        void Apply();
        void Remove();
        void Stack(IStatusEffect<T> other);
        bool Tick(float deltaTime);
    }

    public abstract class StatusEffectBase<T> : IStatusEffect<T> where T : IStatusEffect<T>
    {
        public T Context { get; private set; }

        public virtual IStatusEffect<T> Clone(T ctx)
        {
            var clone = (IStatusEffect<T>)MemberwiseClone();
            Context = ctx;
            return clone;
        }

        public abstract void Apply();
        public abstract void Remove();
        public abstract void Stack(IStatusEffect<T> other);
        public abstract bool Tick(float deltaTime);
    }
}