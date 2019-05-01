
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIPlanner
{
    /// <summary>
    /// A Compound task is a list of Methods. At execute time, the appropriate
    /// method is chosen based on world state and used to create a plan.
    /// </summary>
    public class CompoundTask : Task
    {
        internal Domain domain;
        internal List<Method> methods = new List<Method>();

        public override string ToString()
        {
            return $"Compound Task: {this.name}:\n    {string.Join("\n    ", (from i in methods select i.ToString()))}\n";
        }

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
        public CompoundTask Conditions(params Condition[] conditions)
        {
            var m = methods.Last();
            m.conditions.AddRange(conditions);
            return this;
        }

        /// <summary>
        /// Adds named tasks to the last Method created by the DefineMethod 
        /// call.
        /// </summary>
        public CompoundTask Tasks(params ActionDelegate[] tasks)
        {
            var m = methods.Last();
            foreach (var i in tasks)
            {
                var name = i.Method.Name;
                if (domain.tasks.ContainsKey(name))
                    m.tasks.Add(new TaskReference(name));
                else
                    throw new Exception($"Task is not defined: {name}");
            }
            return this;
        }

        internal Method FindSatisfiedMethod(List<StateVariable> state)
        {
            foreach (var m in methods)
            {
                var valid = false;
                foreach (var s in m.conditions)
                {
                    valid = s.IsTrue(state);
                }
                if (valid) return m;
            }
            return null;
        }
    }
}