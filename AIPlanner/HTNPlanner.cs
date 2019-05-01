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
    public static partial class HTNPlanner
    {
        [ThreadStatic] static Stack<PlannerState> history;
        [ThreadStatic] static List<Task> taskQueue;
        [ThreadStatic] static List<StateVariable> states;
        struct PlannerState
        {
            public List<float> state;
            public List<PrimitiveTask> plan;
            public List<Task> queue;
        }

        static object me = new object();

        /// <summary>
        /// Creates the plan based on current world state. A plan is a list of
        /// Primitive Tasks.
        /// </summary>
        /// <returns>The plan.</returns>
        /// <param name="currentState">Current state.</param>
        /// <param name="domain">Domain.</param>
        static public bool CreatePlan(Domain domain, List<StateVariable> worldState, Plan plan)
        {
            lock (me)
            {
                plan.Clear();
                if (history == null) history = new Stack<PlannerState>();
                if (taskQueue == null) taskQueue = new List<Task>();
                if (states == null) states = new List<StateVariable>();
                states.Clear();
                taskQueue.Clear();
                history.Clear();
                taskQueue.Add(domain.root);

                states.AddRange(worldState);

                while (taskQueue.Count > 0)
                {
                    var task = taskQueue[0];
                    taskQueue.RemoveAt(0);
                    if (task is CompoundTask)
                    {
                        var compoundTask = task as CompoundTask;
                        var method = compoundTask.FindSatisfiedMethod(states);
                        if (method != null)
                        {
                            SaveHistory(taskQueue, plan, states);
                            foreach (var i in method.tasks)
                            {
                                taskQueue.Add(domain.tasks[i.name]);
                            }
                        }
                        else
                        {
                            if (!RestoreHistory(taskQueue, plan, states))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        var primitiveTask = task as PrimitiveTask;
                        if (primitiveTask.ConditionsAreValid(states))
                        {
                            primitiveTask.ApplyEffects(states);
                            plan.Add(primitiveTask);
                        }
                        else
                        {
                            if (!RestoreHistory(taskQueue, plan, states))
                            {
                                return false;
                            }
                        }
                    }
                }
                history.Clear();
                return true;
            }
        }

        static bool RestoreHistory(List<Task> taskQueue, List<PrimitiveTask> plan, List<StateVariable> state)
        {
            if (history.Count == 0) return false;
            var h = history.Pop();
            taskQueue.Clear();
            taskQueue.AddRange(h.queue);
            plan.Clear();
            plan.AddRange(h.plan);
            for (var i = 0; i < state.Count; i++)
                state[i].value = h.state[i];
            ListPool<Task>.Return(h.queue);
            ListPool<PrimitiveTask>.Return(h.plan);
            ListPool<float>.Return(h.state);
            return true;
        }

        static void SaveHistory(List<Task> taskQueue, List<PrimitiveTask> plan, List<StateVariable> state)
        {
            var ps = new PlannerState
            {
                queue = ListPool<Task>.Take(taskQueue),
                plan = ListPool<PrimitiveTask>.Take(plan),
                state = ListPool<float>.Take()
            };
            foreach (var i in state)
            {
                ps.state.Add(i.value);
            }
            history.Push(ps);
        }
    }
}