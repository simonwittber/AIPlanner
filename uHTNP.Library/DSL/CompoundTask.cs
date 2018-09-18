
using System;
using System.Collections.Generic;
using System.Linq;

namespace uHTNP.DSL
{
    public class CompoundTask : Task
    {
        public Domain domain;
        public List<Method> methods = new List<Method>();

        public CompoundTask Conditions(params string[] preconditions)
        {
            var m = new Method();
            methods.Add(m);
            m.preconditions.AddRange(from i in preconditions select domain.GetPrecondition(i));
            return this;
        }

        public CompoundTask Tasks(params string[] tasks)
        {
            var m = methods.Last();
            foreach (var i in tasks)
            {
                if (domain.tasks.ContainsKey(i))
                    m.tasks.Add(new TaskReference(i));
                else
                    throw new Exception($"Task is not defined: {i}");
            }
            return this;
        }

        public Method FindSatisfiedMethod(WorldState state)
        {
            foreach (var m in methods)
            {
                var valid = domain.PreconditionsAreValid(state, m.preconditions);
                if (valid) return m;
            }
            return null;
        }
    }
}