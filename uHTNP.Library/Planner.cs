using System;
using System.Collections;
using System.Collections.Generic;
using uHTNP.DSL;


namespace uHTNP
{
    public static class Planner
    {
        [ThreadStatic] static readonly Stack<PlanState> history = new Stack<PlanState>();

        struct PlanState
        {
            public WorldState state;
            public PrimitiveTask[] plan;
            public Task[] queue;
        }

        static public List<PrimitiveTask> CreatePlan(WorldState currentState, Domain domain)
        {
            var taskQueue = new List<Task>();
            taskQueue.Add(domain.root);
            var plan = new List<PrimitiveTask>();
            history.Clear();
            var state = currentState.Copy();

            while (taskQueue.Count > 0)
            {
                var task = taskQueue[0];
                taskQueue.RemoveAt(0);
                if (task is CompoundTask)
                {
                    var compoundTask = task as CompoundTask;
                    var method = compoundTask.FindSatisfiedMethod(state);
                    if (method != null)
                    {
                        SaveHistory(taskQueue, plan, state);
                        foreach (var i in method.tasks)
                        {
                            taskQueue.Add(domain.tasks[i.name]);
                        }
                    }
                    else
                    {
                        if (!RestoreHistory(taskQueue, plan, state))
                            return null;
                    }
                }
                else
                {
                    var primitiveTask = task as PrimitiveTask;
                    if (state.PreconditionsAreValid(primitiveTask.preconditions))
                    {
                        state.ApplyEffects(primitiveTask.effects);
                        plan.Add(primitiveTask);
                    }
                    else
                    {
                        if (!RestoreHistory(taskQueue, plan, state))
                            return null;
                    }
                }
            }
            history.Clear();
            return plan;
        }

        static bool RestoreHistory(List<Task> taskQueue, List<PrimitiveTask> plan, WorldState state)
        {
            if (history.Count == 0) return false;
            var h = history.Pop();
            taskQueue.Clear();
            taskQueue.AddRange(h.queue);
            plan.Clear();
            plan.AddRange(h.plan);
            state.Copy(h.state);
            return true;
        }

        static void SaveHistory(List<Task> taskQueue, List<PrimitiveTask> plan, WorldState state)
        {
            history.Push(new PlanState { queue = taskQueue.ToArray(), plan = plan.ToArray(), state = state.Copy() });
        }
    }
}