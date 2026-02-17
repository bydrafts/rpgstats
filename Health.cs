using System;
using UnityEngine;

namespace Drafts.Rpg
{
    [Serializable]
    public class Health
    {
        public class Changes
        {
            public Health Health;
            public object Source;
            public int Current;
            public int Delta;
            public bool Fatal;
            public bool Revive;
        }

        [field: SerializeField] public int Max { get; private set; }
        [field: SerializeField] public int Current { get; private set; }
        public float Normalized => Current / (float)Max;
        public event Action<Changes> OnChanged;

        public void Awake() => FullHeal(null);
        public void FullHeal(object source) => Set(source, Max);

        public void SetMax(object source, int value) => Max = value;
        public Changes Add(object source, int value) => Set(source, Current + value);

        public Changes Set(object source, int value)
        {
            var next = Mathf.Clamp(value, 0, Max);
            var delta = next - Current;

            Current = next;

            var changes = new Changes
            {
                Health = this,
                Source = source,
                Current = next,
                Delta = delta,
                Fatal = delta < 0 && next == 0,
                Revive = delta > 0 && next > 0
            };

            if (delta != 0)
                OnChanged?.Invoke(changes);
            return changes;
        }
    }
}