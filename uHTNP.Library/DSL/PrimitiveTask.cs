using System.Collections.Generic;
using System.Linq;

namespace uHTNP.DSL
{
    public class PrimitiveTask : Task
    {
        internal Domain domain;
        internal List<Precondition> preconditions = new List<Precondition>();
        internal Action action = new Action();
        internal List<Effect> effects = new List<Effect>();

        /// <summary>
        /// Adds named conditions to the task. Conditions are checked before
        /// executing this task.
        /// </summary>
        public PrimitiveTask Conditions(params string[] preconditions)
        {
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
            effects.Add(new Effect { name = stateName, value = true });
            return this;
        }

        /// <summary>
        /// When the task succeeds, this state variable will be set to false.
        /// </summary>
        public PrimitiveTask Unset(string stateName)
        {
            effects.Add(new Effect { name = stateName, value = false });
            return this;
        }
    }
}