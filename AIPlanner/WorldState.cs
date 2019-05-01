using System.Collections.Generic;

namespace AIPlanner
{
    public class WorldState : List<StateVariable>
    {
        public WorldState(IEnumerable<StateVariable> collection) : base(collection)
        {
        }
    }
}