using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drafts.Rpg
{
    public enum StatBonus
    {
        Add,
        Mult,
    }

    [Serializable]
    public class Stat
    {
        public Stat() { }
        public Stat(float @base) => Total = Base = @base;
        public Stat(string name) => this.name = name;

        [SerializeField] private string name;
        private Dictionary<object, float> Bonus { get; } = new();
        private Dictionary<object, float> Mult { get; } = new();

        public string Name => name;
        [field: SerializeField] public float Base { get; private set; }
        [field: SerializeField] public float Total { get; protected set; }
        public int IntTotal => Mathf.RoundToInt(Total);
        public event Action<float> OnChanged;

        public virtual void Recalculate()
        {
            Total = Base;
            foreach (var v in Bonus) Total += v.Value;
            foreach (var v in Mult) Total *= v.Value;
            OnChanged?.Invoke(Total);
        }

        public void Reset(float @base)
        {
            Bonus.Clear();
            Mult.Clear();
            SetBase(@base);
        }

        public void SetBase(float value)
        {
            Base = value;
            Recalculate();
        }

        public virtual void AddBonus(IBonus<StatBonus, float> bonus)
        {
            switch (bonus.Type)
            {
                case StatBonus.Add: Bonus[bonus.Source] = bonus.Value; break;
                case StatBonus.Mult: Mult[bonus.Source] = bonus.Value; break;
                default: throw new ArgumentOutOfRangeException();
            }

            Recalculate();
        }

        public virtual void RemoveBonus(IBonus<StatBonus, float> bonus)
        {
            if (!Bonus.Remove(bonus.Source) &&
                !Mult.Remove(bonus.Source)) return;
            Recalculate();
        }

        public static implicit operator float(Stat s) => s.Total;
        public static implicit operator int(Stat s) => s.IntTotal;
    }
}