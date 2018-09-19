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

    public class PlanRunner
    {
        readonly Queue<PrimitiveTask> tasks;
        readonly Domain domain;

        public PlanRunner(Domain domain, List<PrimitiveTask> plan)
        {
            tasks = new Queue<PrimitiveTask>(plan);
            this.domain = domain;
        }

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