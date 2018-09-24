using System;
using System.Collections.Generic;
using uHTNP.DSL;

namespace uHTNP
{
    public class WorldState
    {
        internal Dictionary<string, bool> states = new Dictionary<string, bool>();

        /// <summary>
        /// Get the state specified by name. All states default to false.
        /// </summary>
        public bool Get(string name)
        {
            bool value = false;
            if (states.TryGetValue(name, out value))
                return value;
            return false;
        }

        /// <summary>
        /// Set the state specified by name to a value.
        /// </summary>
        public void Set(string name, bool value)
        {
            states[name] = value;
        }

        internal bool PreconditionsAreValid(List<Precondition> preconditions)
        {
            foreach (var p in preconditions)
            {
                if (!p.value == Get(p.name)) return false;
                if (p.proceduralPrecondition != null)
                    if (!p.proceduralPrecondition.Invoke()) return false;
            }
            return true;
        }

        internal void ApplyEffects(List<Effect> effects)
        {
            foreach (var i in effects)
                states[i.name] = i.value;
        }

        internal WorldState Copy()
        {
            var w = new WorldState();
            foreach (var kv in states)
                w.states[kv.Key] = kv.Value;
            return w;
        }

        internal void Copy(WorldState state)
        {
            foreach (var kv in state.states)
                states[kv.Key] = kv.Value;
        }


    }
}