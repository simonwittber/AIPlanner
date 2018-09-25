using System;
using System.Collections;
using System.Collections.Generic;
using AIPlanner.DSL;


namespace AIPlanner
{
    /// <summary>
    /// Planner is a static class used to create a plan (List of tasks) in a 
    /// domain.
    /// </summary>
    public static class HTNPlanner
    {
        [ThreadStatic] static Stack<PlanState> history;

        struct PlanState
        {
            public WorldState state;
            public PrimitiveTask[] plan;
            public Task[] queue;
        }

        /// <summary>
        /// Creates the plan based on current world state. A plan is a list of
        /// Primitive Tasks.
        /// </summary>
        /// <returns>The plan.</returns>
        /// <param name="currentState">Current state.</param>
        /// <param name="domain">Domain.</param>
        static public List<PrimitiveTask> CreatePlan(WorldState currentState, Domain domain)
        {
            domain.UpdateWorldState(currentState);
            var taskQueue = new List<Task> { domain.root };
            var plan = new List<PrimitiveTask>();
            if (history == null) history = new Stack<PlanState>();
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