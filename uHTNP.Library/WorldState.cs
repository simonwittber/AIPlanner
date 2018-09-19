using System;
using System.Collections.Generic;
using uHTNP.DSL;

namespace uHTNP
{
    public class WorldState
    {
        public Dictionary<string, bool> states = new Dictionary<string, bool>();

        public bool PreconditionsAreValid(List<Precondition> preconditions)
        {
            foreach (var p in preconditions)
                if (!p.value == Get(p.name)) return false;
            return true;
        }

        public void ApplyEffects(List<Effect> effects)
        {
            foreach (var i in effects)
                states[i.name] = i.value;
        }

        public WorldState Copy()
        {
            var w = new WorldState();
            foreach (var kv in states)
                w.states[kv.Key] = kv.Value;
            return w;
        }

        public void Copy(WorldState state)
        {
            foreach (var kv in state.states)
                states[kv.Key] = kv.Value;
        }

        public bool Get(string name)
        {
            bool value = false;
            if (states.TryGetValue(name, out value))
                return value;
            return false;
        }

        public void Set(string name, bool value)
        {
            states[name] = value;
        }
    }
}