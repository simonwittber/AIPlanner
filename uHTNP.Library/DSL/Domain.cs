using System;
using System.Collections.Generic;

namespace uHTNP.DSL
{
    public class Domain : IDisposable
    {
        public Dictionary<string, Task> tasks = new Dictionary<string, Task>();
        public Dictionary<string, Precondition> preconditions = new Dictionary<string, Precondition>();
        public Dictionary<string, Action> actions = new Dictionary<string, Action>();

        public Precondition GetPrecondition(string name)
        {
            Precondition precondition;
            if (!preconditions.TryGetValue(name, out precondition))
                precondition = preconditions[name] = new Precondition() { name = name };
            return precondition;
        }

        public Action GetAction(string name)
        {
            Action action;
            if (!actions.TryGetValue(name, out action))
                action = actions[name] = new Action() { name = name };
            return action;
        }

        public Task root;

        static Domain active = null;

        public static Domain New()
        {
            if (active != null) throw new Exception("Must dispose current domain before calling New.");
            active = new Domain();
            return active;
        }

        public static CompoundTask DefineCompound(string name)
        {
            var t = new CompoundTask() { name = name, domain = active };
            active.tasks.Add(name, t);
            return t;
        }

        public static PrimitiveTask DefinePrimitive(string name)
        {
            var p = new PrimitiveTask() { name = name, domain = active };
            active.tasks.Add(name, p);
            return p;
        }

        public static void SetRootTask(string name)
        {
            active.root = active.tasks[name];
        }

        public bool PreconditionsAreValid(WorldState state, List<Precondition> preconditions)
        {
            foreach (var p in preconditions)
                if (!p.value == state.Get(p.name)) return false;
            return true;
        }

        public void Dispose()
        {
            active = null;
        }
    }
}