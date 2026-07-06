using System;
using System.Collections.Generic;

namespace Drafts.Rpg
{
    [Serializable]
    public class BoolStat
    {
        private HashSet<object> _modifiers = new();
        public bool Value => _modifiers.Count > 0;
        public event Action<bool> OnChanged;

        public void Add(object modifier)
        {
            if (_modifiers.Add(modifier) && _modifiers.Count == 1)
                OnChanged?.Invoke(true);
        }

        public void Remove(object modifier)
        {
            if (_modifiers.Remove(modifier) && _modifiers.Count == 0)
                OnChanged?.Invoke(false);
        }
    }
}