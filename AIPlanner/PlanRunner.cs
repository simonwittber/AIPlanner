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
        NoPlan,
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
        /// <summary>
        /// The plan failed, usually due to a change in state effecting required
        /// preconditions for a task.
        /// </summary>
        Failed
    }

    /// <summary>
    /// The Plan Runner takes a plan (List of Primitive Tasks) and executes the
    /// plan, checking preconditions and applying effects of actions as needed.
    /// </summary>

    public class PlanRunner
    {
        public Task ActiveTask { get; private set; }
        public ActionState ActiveState { get; private set; }
        public PlanState PlanState { get; private set; } = PlanState.Waiting;

        readonly Queue<PrimitiveTask> tasks;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:uHTNP.PlanRunner"/> 
        /// class for a given domain and list of tasks.
        /// </summary>
        /// <param name="domain">Domain.</param>
        /// <param name="plan">Plan.</param>
        public PlanRunner(Plan plan)
        {
            tasks = new Queue<PrimitiveTask>();
            foreach (var i in plan)
                tasks.Enqueue(i);
        }

        public void Reset(Plan plan)
        {
            tasks.Clear();
            foreach (var i in plan)
                tasks.Enqueue(i);
            PlanState = PlanState.Waiting;
        }

        /// <summary>
        /// Execute the plan, returning current plan state. If the PlanState is 
        /// InProgress, Execute will need to be called again during the next
        /// update tick.
        /// </summary>
        public void Execute(List<StateVariable> worldState)
        {
            while (tasks.Count > 0)
            {
                var task = tasks.Dequeue();
                ActiveTask = task;
                if (!task.ConditionsAreValid(worldState))
                {
                    ActiveTask = task;
                    PlanState = PlanState.Failed;
                    return;
                }
                switch (ExecuteTask(task))
                {
                    case ActionState.Error:
                        PlanState = PlanState.Failed;
                        return;
                    case ActionState.InProgress:
                        tasks.Enqueue(task);
                        PlanState = PlanState.InProgress;
                        return;
                    case ActionState.Success:
                        ApplyEffects(worldState, task.effects);
                        continue;
                }
            }
            PlanState = PlanState.Completed;
        }

        private void ApplyEffects(List<StateVariable> worldState, List<Effect> effects)
        {
            if (effects != null)
                foreach (var i in effects)
                {
                    i.fn(worldState[i.variable.index]);
                }
        }

        ActionState ExecuteTask(PrimitiveTask task)
        {
            ActiveTask = task;
            ActiveState = task.action();
            return ActiveState;
        }
    }
}