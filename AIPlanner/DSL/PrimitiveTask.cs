using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanner.DSL
{
    /// <summary>
    /// A primitive task is an action that is executed if preconditions are 
    /// satisfied. It then applies it's effects to the world state.
    /// </summary>
    public class PrimitiveTask : Task
    {
        internal Domain domain;
        internal List<Precondition> preconditions;
        internal Action action = new Action();
        internal List<Effect> effects;
        internal ProceduralCost proceduralCost;
        internal float cost;

        /// <summary>
        /// Adds named conditions to the task. Conditions are checked before
        /// executing this task.
        /// </summary>
        public PrimitiveTask Conditions(params string[] preconditions)
        {
            if (this.preconditions == null) this.preconditions = new List<Precondition>(1);
            this.preconditions.AddRange(from i in preconditions select domain.GetPrecondition(i));
            return this;
        }

        /// <summary>
        /// Sets the named action that this primitive task will run.
        /// </summary>
        public PrimitiveTask Actions(string actionName)
        {
            action = domain.GetAction(actionName);
            return this;
        }

        /// <summary>
        /// When the task succeeds, this state variable will be set to true.
        /// </summary>
        public PrimitiveTask Set(string stateName)
        {
            if (effects == null) effects = new List<Effect>(1);
            effects.Add(new Effect { name = stateName, value = true });
            return this;
        }

        /// <summary>
        /// When the task succeeds, this state variable will be set to false.
        /// </summary>
        public PrimitiveTask Unset(string stateName)
        {
            if (effects == null) effects = new List<Effect>(1);
            effects.Add(new Effect { name = stateName, value = false });
            return this;
        }

        /// <summary>
        /// Attach a cost value to this task.
        /// </summary>
        public PrimitiveTask Cost(float cost)
        {
            this.cost = cost;
            return this;
        }

        /// <summary>
        /// Attach a procedural cost to this task, which is bound and calculated
        /// at runtime.
        /// </summary>
        public PrimitiveTask Cost(string name)
        {
            proceduralCost = domain.GetCost(name);
            return this;
        }
    }
}