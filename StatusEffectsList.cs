using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using NotifyAgs = System.Collections.Specialized.NotifyCollectionChangedEventArgs;
using NotifyAction = System.Collections.Specialized.NotifyCollectionChangedAction;

namespace Drafts.Rpg
{
    public enum StatusEffectEvent
    {
        Add, Stack, Remove,
    }

    public class StatusEffectList<T> : IReadOnlyDictionary<object, T>, INotifyCollectionChanged
        where T : IStatusContext<T>
    {
        protected readonly Dictionary<object, T> List = new();
        private readonly HashSet<object> _toRemove = new();
        public event Action<StatusEffectEvent, T> OnChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public virtual void Add(T ctx)
        {
            if (List.TryGetValue(ctx.Key, out var existing))
            {
                existing.Effect.Stack(existing, ctx);
                OnChanged?.Invoke(StatusEffectEvent.Stack, existing);
                return;
            }

            List[ctx.Key] = ctx;
            ctx.Effect.Apply(ctx);
            OnChanged?.Invoke(StatusEffectEvent.Add, ctx);
            CollectionChanged?.Invoke(this, new NotifyAgs(NotifyAction.Add, ctx));
        }

        public virtual void Remove(object key)
        {
            if (!List.TryGetValue(key, out var ctx)) return;
            ctx.Effect.Remove(ctx);
            List.Remove(key);
            OnChanged?.Invoke(StatusEffectEvent.Remove, ctx);
            CollectionChanged?.Invoke(this, new NotifyAgs(NotifyAction.Remove, ctx));
        }

        public virtual void Tick(float deltaTime)
        {
            _toRemove.Clear();

            foreach (var ctx in List.Values)
                if (!ctx.Effect.Tick(ctx, deltaTime))
                    _toRemove.Add(ctx.Key);

            foreach (var key in _toRemove)
                Remove(key);
        }

        public int Count => List.Count;
        public bool ContainsKey(object key) => List.ContainsKey(key);
        public bool TryGetValue(object key, out T value) => List.TryGetValue(key, out value);
        public T this[object key] => List[key];
        public IEnumerable<object> Keys => List.Keys;
        public IEnumerable<T> Values => List.Values;
        public IEnumerator<KeyValuePair<object, T>> GetEnumerator() => List.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}