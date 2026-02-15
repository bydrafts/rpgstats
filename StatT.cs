using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drafts.Rpg
{
    [Serializable]
    public abstract class Stat<T>
    {
        protected abstract T Resolve(T a, T b);
        protected Stat(T @base) => Base = @base;

        [field: SerializeField] public T Base { get; set; }
        private Dictionary<object, T> Mod { get; } = new();
        public T Total { get; private set; }
        public event Action<T> OnChanged;

        public void AddMod(IBonus<T> bonus)
        {
            Mod[bonus.Source] = bonus.Value;
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public void RemoveMod(IBonus<T> value)
        {
            if (!Mod.Remove(value)) return;
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public void Recalculate()
        {
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public T GetTotal()
        {
            var total = Base;
            foreach (var v in Mod) total = Resolve(total, v.Value);
            return total;
        }

        public static implicit operator T(Stat<T> s) => s.GetTotal();
    }
}