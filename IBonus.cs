namespace Drafts.Rpg
{
    public interface IBonus<out TValue>
    {
        public object Source => this;
        public TValue Value { get; }
    }
    
    public interface IBonus<out TType, out TValue>
    {
        public object Source => this;
        public TType Type { get; }
        public TValue Value { get; }
    }
}