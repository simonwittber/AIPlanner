using System;
using System.Collections.Generic;
using System.Linq;
using AIPlanner.DSL;


namespace AIPlanner
{
    /// <summary>
    /// A GOAP.
    /// </summary>
    public static class GOAPlanner
    {
        /// <summary>
        /// Creates the plan.
        /// </summary>
        /// <returns>The plan.</returns>
        /// <param name="currentState">Current state.</param>
        /// <param name="goalState">Goal state.</param>
        /// <param name="domain">Domain.</param>
        static public List<PrimitiveTask> CreatePlan(WorldState currentState, WorldState goalState, Domain domain)
        {
            domain.UpdateWorldState(currentState);
            var plan = new List<PrimitiveTask>();
            var tasks = (from i in domain.tasks.Values where i is PrimitiveTask select i as PrimitiveTask).ToList();
            var graph = new AStar();
            graph.GetConnectedNodes = GetConnectedNodes;
            graph.CalculateMoveCost = CalculateMoveCost;
            graph.Route(currentState, goalState, tasks, plan);
            return plan;
        }

        static IEnumerable<PrimitiveTask> GetConnectedNodes(WorldState state, PrimitiveTask task, List<PrimitiveTask> tasks)
        {
            foreach (var i in tasks)
            {
                if (state.PreconditionsAreValid(i.preconditions))
                    yield return i;
            }
        }

        static float CalculateMoveCost(WorldState state, PrimitiveTask src, PrimitiveTask dst)
        {
            return dst.cost + (dst.proceduralCost == null ? 0 : dst.proceduralCost.costDelegate.Invoke(state));
        }
    }
}