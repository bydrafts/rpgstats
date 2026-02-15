using System;
using System.Collections;
using System.Collections.Generic;

namespace Drafts.Rpg
{
    public enum StatuEffectEvent
    {
        Add,
        Stack,
        Remove,
    }

    public class StatuEffectList<T> : IReadOnlyDictionary<T, T> where T : IStatusEffect<T>
    {
        public T Context { get; }
        private readonly Dictionary<T, T> _effects = new();
        private readonly HashSet<T> _toRemove = new();
        public event Action<StatuEffectEvent, T> OnChanged;

        public StatuEffectList(T context) => Context = context;

        public void Add(T status)
        {
            if (status.Context != null)
                throw new Exception("Context is not null. You should not use a clone in Add nor Remove");

            if (_effects.TryGetValue(status, out var effect))
            {
                effect.Stack(status);
                OnChanged?.Invoke(StatuEffectEvent.Stack, effect);
            }
            else
            {
                _effects[status] = effect = (T)status.Clone(Context);
                effect.Apply();
                OnChanged?.Invoke(StatuEffectEvent.Add, effect);
            }
        }

        public void Remove(T status)
        {
            if (status.Context != null)
                throw new Exception("Context is not null. You should not use a clone in Add nor Remove");

            if (!_effects.TryGetValue(status, out var effect)) return;
            effect.Remove();
            _effects.Remove(status);
            OnChanged?.Invoke(StatuEffectEvent.Remove, effect);
        }

        public void Tick(float deltaTime)
        {
            _toRemove.Clear();

            foreach (var (status, effect) in _effects)
                if (!effect.Tick(deltaTime))
                    _toRemove.Add(status);

            foreach (var status in _toRemove)
                Remove(status);
        }

        public int Count => _effects.Count;
        public bool ContainsKey(T key) => _effects.ContainsKey(key);
        public bool TryGetValue(T key, out T value) => _effects.TryGetValue(key, out value);
        public T this[T key] => _effects[key];
        public IEnumerable<T> Keys => _effects.Keys;
        public IEnumerable<T> Values => _effects.Values;
        public IEnumerator<KeyValuePair<T, T>> GetEnumerator() => _effects.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}