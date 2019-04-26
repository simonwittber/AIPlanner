using System;

namespace AIPlanner
{
    public class Effect
    {
        public StateVariable variable;
        public Action<StateVariable> fn;
    }
}