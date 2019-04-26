using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanner
{
    /// <summary>
    /// A primitive task is an action that is executed if preconditions are 
    /// satisfied. It then applies it's effects to the world state.
    /// </summary>
    [System.Serializable]
    public class PrimitiveTask : Task
    {
        internal Domain domain;
        internal List<Condition> conditions;
        internal ActionDelegate action;
        internal List<Effect> effects;
        internal float cost;

        /// <summary>
        /// Adds named conditions to the task. Conditions are checked before
        /// executing this task.
        /// </summary>
        public PrimitiveTask Conditions(params Condition[] conditions)
        {
            if (this.conditions == null) this.conditions = new List<Condition>(1);
            this.conditions.AddRange(conditions);
            return this;
        }

        public bool ConditionsAreValid(StateVariable[] state)
        {
            foreach (var i in conditions)
            {
                if (!i.IsTrue(state)) return false;
            }
            return true;
        }

        /// <summary>
        /// Sets the named action that this primitive task will run.
        /// </summary>
        public PrimitiveTask Actions(ActionDelegate fn)
        {
            action = fn;
            return this;
        }

        /// <summary>
        /// When the task succeeds, this state variable will be set to true.
        /// </summary>
        public PrimitiveTask Set(params Effect[] effects)
        {
            if (this.effects == null) this.effects = new List<Effect>(effects.Length);
            this.effects.AddRange(effects);
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

        public void ApplyEffects(StateVariable[] state)
        {

        }

    }
}