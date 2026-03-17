using System;
using System.Collections;
using System.Collections.Generic;

namespace Drafts.Rpg
{
    public enum StatusEffectEvent
    {
        Add,
        Stack,
        Remove,
    }

    public class StatusEffectList<T, TCtx> : IReadOnlyDictionary<object, T> where T : IStatusEffect<TCtx>
    {
        public TCtx Context { get; }
        private readonly Dictionary<object, T> _effects = new();
        private readonly HashSet<object> _toRemove = new();
        public event Action<StatusEffectEvent, T> OnChanged;

        public StatusEffectList(TCtx context) => Context = context;

        public void Add(T status)
        {
            if (_effects.TryGetValue(status.Key, out var effect))
            {
                effect.Stack(Context, status);
                OnChanged?.Invoke(StatusEffectEvent.Stack, effect);
                return;
            }

            _effects[status.Key] = status;
            status.Apply(Context);
            OnChanged?.Invoke(StatusEffectEvent.Add, status);
        }

        public void Remove(object key)
        {
            if (!_effects.TryGetValue(key, out var effect)) return;
            effect.Remove(Context);
            _effects.Remove(key);
            OnChanged?.Invoke(StatusEffectEvent.Remove, effect);
        }

        public void Tick(float deltaTime)
        {
            _toRemove.Clear();

            foreach (var (key, effect) in _effects)
                if (!effect.Tick(Context, deltaTime))
                    _toRemove.Add(key);

            foreach (var key in _toRemove)
                Remove(key);
        }

        public int Count => _effects.Count;
        public bool ContainsKey(object key) => _effects.ContainsKey(key);
        public bool TryGetValue(object key, out T value) => _effects.TryGetValue(key, out value);
        public T this[object key] => _effects[key];
        public IEnumerable<object> Keys => _effects.Keys;
        public IEnumerable<T> Values => _effects.Values;
        public IEnumerator<KeyValuePair<object, T>> GetEnumerator() => _effects.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}