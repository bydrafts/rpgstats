using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drafts.Rpg
{
    [Serializable]
    public abstract class GenericStat<TVal, TType>
    {
        protected GenericStat(string name, TVal min, TVal max, TVal @base)
        {
            this.name = name;
            Min = min;
            Max = max;
            Total = Base = @base;
        }

        protected Dictionary<TType, Dictionary<object, TVal>> Bonus { get; set; } = new();

        [SerializeField] private string name;
        public string Name => name;
        [field: SerializeField] public TVal Base { get; private set; }
        [field: SerializeField] public TVal Total { get; private set; }
        [field: SerializeField] public TVal Min { get; private set; }
        [field: SerializeField] public TVal Max { get; private set; }
        public event Action<TVal> OnChanged;

        public abstract TVal GetTotal();

        public void Recalculate()
        {
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public void Reset(TVal @base)
        {
            Bonus.Clear();
            SetBase(@base);
        }

        public void SetBase(TVal value)
        {
            Base = value;
            Recalculate();
        }

        public void SetBonus(object src, TType type, TVal value)
        {
            Bonus[type][src] = value;
            Recalculate();
        }

        public void RemoveBonus(object src, TType type)
        {
            if (Bonus[type].Remove(src)) Recalculate();
        }

        public TVal GetBonus(object src, TType type) => !Bonus.TryGetValue(type, out var d) ? default : d.GetValueOrDefault(src);

        public static implicit operator TVal(GenericStat<TVal, TType> s) => s.Total;
    }
}