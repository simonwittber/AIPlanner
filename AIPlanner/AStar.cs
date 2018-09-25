using System;
using System.Collections.Generic;
using System.Linq;
using AIPlanner.DSL;

namespace AIPlanner
{
    class AStar
    {
        readonly Dictionary<PrimitiveTask, float> g;
        readonly Dictionary<PrimitiveTask, PrimitiveTask> parent;
        readonly Dictionary<PrimitiveTask, bool> inPath;
        readonly Queue<PrimitiveTask> path;
        readonly HashList<PrimitiveTask> openset;
        readonly HashSet<PrimitiveTask> closedset;

        public System.Func<WorldState, PrimitiveTask, List<PrimitiveTask>, IEnumerable<PrimitiveTask>> GetConnectedNodes;
        public System.Func<WorldState, PrimitiveTask, PrimitiveTask, float> CalculateMoveCost;

        public AStar()
        {
            g = new Dictionary<PrimitiveTask, float>();
            parent = new Dictionary<PrimitiveTask, PrimitiveTask>();
            inPath = new Dictionary<PrimitiveTask, bool>();
            openset = new HashList<PrimitiveTask>();
            closedset = new HashSet<PrimitiveTask>();
            path = new Queue<PrimitiveTask>();
        }

        public bool Route(WorldState start, WorldState end, List<PrimitiveTask> nodes, List<PrimitiveTask> route)
        {
            route.Clear();
            if (start == null || end == null)
            {
                return false;
            }

            for (int i = 0, nodesLength = nodes.Count; i < nodesLength; i++)
            {
                var s = nodes[i];
                g[s] = 0f;
                parent[s] = null;
                inPath[s] = false;
            }

            openset.Clear();
            closedset.Clear();
            path.Clear();

            var current = new PrimitiveTask { name = "start", effects = start.ToEffectsList() };
            openset.Add(current);
            var state = start.Copy();
            while (openset.Count > 0)
            {
                current = openset[0];
                state.ApplyEffects(current.effects);

                var goalIsSatisfied = state.Satisfies(end);
                if (goalIsSatisfied)
                {
                    while (parent[current] != null)
                    {
                        path.Enqueue(current);
                        inPath[current] = true;
                        current = parent[current];
                        if (path.Count >= nodes.Count)
                        {
                            return false;
                        }
                    }
                    inPath[current] = true;
                    path.Enqueue(current);
                    while (path.Count > 0)
                    {
                        route.Add(path.Dequeue());
                    }
                    return true;
                }
                openset.Remove(current);
                closedset.Add(current);
                var connectedNodes = GetConnectedNodes(state, current, nodes);
                foreach (var node in connectedNodes)
                {
                    if (closedset.Contains(node))
                        continue;
                    if (openset.Contains(node))
                    {
                        var new_g = g[current] + CalculateMoveCost(state, current, node);
                        if (g[node] > new_g)
                        {
                            g[node] = new_g;
                            parent[node] = current;
                        }
                    }
                    else
                    {
                        g[node] = g[current] + CalculateMoveCost(state, current, node);
                        parent[node] = current;
                        openset.Add(node);
                    }
                }
            }
            return false;
        }
    }
}