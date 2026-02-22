using System;
using UnityEngine;

namespace Drafts.Rpg
{
    public enum StatBonus
    {
        Add,
        Mult,
    }

    [Serializable]
    public class Stat : GenericStat<float, StatBonus>
    {
        public Stat(string name, float min, float max, float @base = 0)
            : base(name, min, max, @base) => Bonus = new()
        {
            { StatBonus.Add, new() },
            { StatBonus.Mult, new() },
        };

        public override float GetTotal()
        {
            var t = Base;
            foreach (var v in Bonus[StatBonus.Add]) t += v.Value;
            foreach (var v in Bonus[StatBonus.Mult]) t *= v.Value;
            return Mathf.Clamp(t, Min, Max);
        }

        public int IntTotal => Mathf.RoundToInt(Total);
        public static implicit operator float(Stat s) => s.Total;
        public static implicit operator int(Stat s) => s.IntTotal;
    }
}