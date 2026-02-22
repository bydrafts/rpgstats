using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drafts.Rpg
{
    [Serializable]
    public abstract class SimpleStat<T>
    {
        protected SimpleStat(string name, T @base)
        {
            this.name = name;
            Total = Base = @base;
        }

        protected  Dictionary<object, T> Bonus { get; set; } = new();

        [SerializeField] private string name;
        public string Name => name;
        [field: SerializeField] public T Base { get; private set; }
        [field: SerializeField] public T Total { get; protected set; }
        public event Action<T> OnChanged;
        
        public abstract T GetTotal();
        public void Recalculate() => OnChanged?.Invoke(Total = GetTotal());
        public void Reset(T @base) { Bonus.Clear(); SetBase(@base); }
        public void SetBase(T value) { Base = value; Recalculate(); }
        public void SetBonus(object src,  T value) { Bonus[src] = value; Recalculate(); }
        public void RemoveBonus(object src) { if (Bonus.Remove(src)) Recalculate(); }
        public T GetBonus(object src) => Bonus.GetValueOrDefault(src);

        public static implicit operator T(SimpleStat<T> s) => s.Total;
    }
}