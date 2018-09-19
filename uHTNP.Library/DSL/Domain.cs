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
                precondition = preconditions[name] = new Precondition { name = name };
            return precondition;
        }

        public Action GetAction(string name)
        {
            Action action;
            if (!actions.TryGetValue(name, out action))
                action = actions[name] = new Action { name = name };
            return action;
        }

        public Task root;

        static Domain active;

        public static Domain New()
        {
            if (active != null) throw new Exception("Must dispose current domain before calling New.");
            active = new Domain();
            return active;
        }

        public static CompoundTask DefineCompoundTask(string name)
        {
            CheckInternalState();
            var t = new CompoundTask { name = name, domain = active };
            active.tasks.Add(name, t);
            return t;
        }

        public static PrimitiveTask DefinePrimitiveTask(string name)
        {
            CheckInternalState();
            var p = new PrimitiveTask { name = name, domain = active };
            active.tasks.Add(name, p);
            return p;
        }

        public static void SetRootTask(string name)
        {
            CheckInternalState();
            active.root = active.tasks[name];
        }

        public void Dispose()
        {
            active = null;
        }

        static void CheckInternalState() {
            if (active == null) throw new Exception("Must call Domain.New before using DSL methods.");
        }
    }
}