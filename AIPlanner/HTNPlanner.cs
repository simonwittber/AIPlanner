using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIPlanner
{
    /// <summary>
    /// Planner is a static class used to create a plan (List of tasks) in a 
    /// domain.
    /// </summary>
    public static class HTNPlanner
    {
        [ThreadStatic] static Stack<PlannerState> history;

        struct PlannerState
        {
            public float[] state;
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
        static public bool CreatePlan(Domain domain)
        {
            var taskQueue = new List<Task> { domain.root };
            domain.plan.Clear();
            if (history == null) history = new Stack<PlannerState>();
            history.Clear();
            var state = (StateVariable[])domain.worldState.Clone();

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
                        SaveHistory(taskQueue, domain.plan, state);
                        foreach (var i in method.tasks)
                        {
                            taskQueue.Add(domain.tasks[i.name]);
                        }
                    }
                    else
                    {
                        if (!RestoreHistory(taskQueue, domain.plan, state))
                        {
                            domain.planState = PlanState.NoPlan;
                            return false;
                        }
                    }
                }
                else
                {
                    var primitiveTask = task as PrimitiveTask;
                    if (primitiveTask.ConditionsAreValid(state))
                    {
                        primitiveTask.ApplyEffects(state);
                        domain.plan.Add(primitiveTask);
                    }
                    else
                    {
                        if (!RestoreHistory(taskQueue, domain.plan, state))
                        {
                            domain.planState = PlanState.NoPlan;
                            return false;
                        }
                    }
                }
            }
            history.Clear();
            domain.planState = PlanState.Waiting;
            return true;
        }

        static bool RestoreHistory(List<Task> taskQueue, List<PrimitiveTask> plan, StateVariable[] state)
        {
            if (history.Count == 0) return false;
            var h = history.Pop();
            taskQueue.Clear();
            taskQueue.AddRange(h.queue);
            plan.Clear();
            plan.AddRange(h.plan);
            for (var i = 0; i < state.Length; i++)
                state[i].value = h.state[i];
            return true;
        }

        static void SaveHistory(List<Task> taskQueue, List<PrimitiveTask> plan, StateVariable[] state)
        {
            history.Push(new PlannerState { queue = taskQueue.ToArray(), plan = plan.ToArray(), state = (from i in state select i.value).ToArray() });
        }
    }
}