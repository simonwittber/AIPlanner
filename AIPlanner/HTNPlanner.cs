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
    public partial class HTNPlanner
    {
        Stack<PlannerState> history;
        List<Task> taskQueue;
        List<StateVariable> states;

        class PlannerState
        {
            public List<float> state = new List<float>();
            public List<PrimitiveTask> plan = new List<PrimitiveTask>();
            public List<Task> queue = new List<Task>();

            public void Clear()
            {
                state.Clear();
                plan.Clear();
                queue.Clear();
            }
        }

        Stack<PlannerState> plannerStatePool = new Stack<PlannerState>();


        /// <summary>
        /// Creates the plan based on current world state. A plan is a list of
        /// Primitive Tasks.
        /// </summary>
        /// <returns>The plan.</returns>
        /// <param name="currentState">Current state.</param>
        /// <param name="domain">Domain.</param>
        public bool CreatePlan(Domain domain, List<StateVariable> worldState, Plan plan)
        {
            var result = _CreatePlan(domain, worldState, plan);
            foreach (var i in history)
            {
                i.Clear();
                plannerStatePool.Push(i);
            }
            history.Clear();
            return result;
        }


        bool _CreatePlan(Domain domain, List<StateVariable> worldState, Plan plan)
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
            return true;
        }

        bool RestoreHistory(List<Task> taskQueue, List<PrimitiveTask> plan, List<StateVariable> state)
        {
            if (history.Count == 0) return false;
            var h = history.Pop();
            taskQueue.Clear();
            taskQueue.AddRange(h.queue);
            plan.Clear();
            plan.AddRange(h.plan);
            for (var i = 0; i < state.Count; i++)
                state[i].value = h.state[i];

            h.Clear();
            plannerStatePool.Push(h);
            return true;
        }

        void SaveHistory(List<Task> taskQueue, List<PrimitiveTask> plan, List<StateVariable> state)
        {
            PlannerState ps;
            if (plannerStatePool.Count > 0)
                ps = plannerStatePool.Pop();
            else
                ps = new PlannerState();

            ps.queue.AddRange(taskQueue);
            ps.plan.AddRange(plan);
            foreach (var i in state)
                ps.state.Add(i.value);
            history.Push(ps);
        }
    }
}