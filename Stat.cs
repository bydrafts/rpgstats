using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drafts.Rpg
{
    [Serializable]
    public class Stat
    {
        public Stat(string name, float @base)
        {
            Name = name;
            Base = @base;
            Total = @base;
        }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public float Base { get; private set; }
        private Dictionary<object, float> Bonus { get; } = new();
        private Dictionary<object, float> Mult { get; } = new();
        public float Total { get; private set; }
        public event Action<float> OnChanged;

        public float GetTotal()
        {
            var total = Base;
            foreach (var v in Bonus) total += v.Value;
            foreach (var v in Mult) total *= v.Value;
            return Mathf.RoundToInt(total);
        }

        public void SetBase(float value)
        {
            Base = value;
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public void AddBonus(IBonus<float> bonus)
        {
            Bonus[bonus.Source] = bonus.Value;
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public void RemoveBonus(IBonus<float> bonus)
        {
            if (!Bonus.Remove(bonus)) return;
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public void AddMult(IBonus<float> bonus)
        {
            Mult[bonus.Source] = bonus.Value;
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public void RemoveMult(IBonus<float> bonus)
        {
            if (!Mult.Remove(bonus)) return;
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public void Recalculate()
        {
            Total = GetTotal();
            OnChanged?.Invoke(Total);
        }

        public int GetIntTotal() => Mathf.RoundToInt(GetTotal());
        public static implicit operator float(Stat s) => s.GetTotal();
    }
}