using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drafts.Rpg
{
    [Serializable]
    public class BoolStat
    {
        [SerializeField] private int count;
        private readonly HashSet<object> _modifiers = new();
        public bool Value => count > 0;
        public event Action<bool> OnChanged;

        public void Add(object modifier)
        {
            if (!_modifiers.Add(modifier)) return;
            count = _modifiers.Count;
            if (_modifiers.Count != 1) return;
            OnChanged?.Invoke(true);
        }

        public void Remove(object modifier)
        {
            if (!_modifiers.Remove(modifier)) return;
            count = _modifiers.Count;
            if (_modifiers.Count != 0) return;
            OnChanged?.Invoke(false);
        }
    }
}