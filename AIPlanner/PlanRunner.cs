using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIPlanner
{
    /// <summary>
    /// Represents the current state of plan execution.
    /// </summary>
    public enum PlanState
    {
        /// <summary>
        /// The plan failed, usually due to a change in state effecting required
        /// preconditions for a task.
        /// </summary>
        Failed,
        /// <summary>
        /// The plan is stil in progress, and Execute needs to be called again
        /// in the next frame.
        /// </summary>
        InProgress,
        /// <summary>
        /// The plan completed successfuly.
        /// </summary>
        Completed,
        /// <summary>
        /// The plan has not started.
        /// </summary>
        Waiting,
        NoPlan
    }

    /// <summary>
    /// The Plan Runner takes a plan (List of Primitive Tasks) and executes the
    /// plan, checking preconditions and applying effects of actions as needed.
    /// </summary>
    public class PlanRunner
    {
        readonly Queue<PrimitiveTask> tasks;
        readonly Domain domain;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:uHTNP.PlanRunner"/> 
        /// class for a given domain and list of tasks.
        /// </summary>
        /// <param name="domain">Domain.</param>
        /// <param name="plan">Plan.</param>
        public PlanRunner(Domain domain)
        {
            tasks = new Queue<PrimitiveTask>(domain.plan);
            this.domain = domain;
        }

        public void Reset()
        {
            tasks.Clear();
            foreach (var i in domain.plan)
                tasks.Enqueue(i);
        }

        /// <summary>
        /// Execute the plan, returning current plan state. If the PlanState is 
        /// InProgress, Execute will need to be called again during the next
        /// update tick.
        /// </summary>
        public PlanState Execute()
        {
            while (tasks.Count > 0)
            {
                var task = tasks.Dequeue();
                switch (ExecuteTask(task))
                {
                    case ActionState.Error:
                        return PlanState.Failed;
                    case ActionState.InProgress:
                        tasks.Enqueue(task);
                        return PlanState.InProgress;
                    case ActionState.Success:
                        ApplyEffects(task.effects);
                        continue;
                }
            }
            return PlanState.Completed;
        }

        private void ApplyEffects(List<Effect> effects)
        {
            foreach (var i in effects)
            {
                i.fn(domain.worldState[i.variable.index]);
            }
        }

        ActionState ExecuteTask(PrimitiveTask task) => task.action();
    }
}