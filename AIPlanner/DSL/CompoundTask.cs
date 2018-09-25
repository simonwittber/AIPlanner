
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanner.DSL
{
    /// <summary>
    /// A Compound task is a list of Methods. At execute time, the appropriate
    /// method is chosen based on world state and used to create a plan.
    /// </summary>
    public class CompoundTask : Task
    {
        internal Domain domain;
        internal List<Method> methods = new List<Method>();

        /// <summary>
        /// Defines a new method on this task. A method is a set of 
        /// preconditions and a list of sub tasks.
        /// </summary>
        public CompoundTask DefineMethod(string name)
        {
            var m = new Method { name = name };
            methods.Add(m);
            return this;
        }

        /// <summary>
        /// Adds named preconditions to the last Method created by the 
        /// DefineMethod call.
        /// </summary>
        public CompoundTask Conditions(params string[] preconditions)
        {
            var m = methods.Last();
            m.preconditions.AddRange(from i in preconditions select domain.GetPrecondition(i));
            return this;
        }

        /// <summary>
        /// Adds named tasks to the last Method created by the DefineMethod 
        /// call.
        /// </summary>
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

        internal Method FindSatisfiedMethod(WorldState state)
        {
            foreach (var m in methods)
            {
                var valid = state.PreconditionsAreValid(m.preconditions);
                if (valid) return m;
            }
            return null;
        }
    }
}