using System;
using System.Collections;
using System.Collections.Generic;
using uHTNP.DSL;

namespace uHTNP
{
    public enum PlanState
    {
        Failed,
        InProgress,
        Completed
    }

    /// <summary>
    /// The Plan Runner takes a plan (List of Primitive Tasks) and executes the
    /// plan, checking preconditions and applying effects of actions as needed.
    /// </summary>
    public class PlanRunner
    {
        readonly Queue<PrimitiveTask> tasks;
        readonly Domain domain;

        public PlanRunner(Domain domain, List<PrimitiveTask> plan)
        {
            tasks = new Queue<PrimitiveTask>(plan);
            this.domain = domain;
        }

        /// <summary>
        /// Execute the plan, returning current plan state. If the PlanState is 
        /// InProgress, Execute will need to be called again during the next
        /// update tick.
        /// </summary>
        public PlanState Execute(WorldState state)
        {
            while (tasks.Count > 0)
            {
                var task = tasks.Dequeue();
                domain.UpdateWorldState(state);
                switch (ExecuteTask(state, task))
                {
                    case ActionState.Error:
                        return PlanState.Failed;
                    case ActionState.InProgress:
                        tasks.Enqueue(task);
                        return PlanState.InProgress;
                    case ActionState.Success:
                        state.ApplyEffects(task.effects);
                        continue;
                }
            }
            return PlanState.Completed;
        }

        ActionState ExecuteTask(WorldState state, PrimitiveTask task) => task.action.actionDelegate.Invoke(state);
    }
}