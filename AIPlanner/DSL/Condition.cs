using System;
using System.Collections.Generic;

namespace AIPlanner
{

    public class Condition
    {
        public StateVariable variable;
        public System.Func<StateVariable, bool> fn;

        public bool IsTrue(List<StateVariable> state)
        {
            return fn(state[variable.index]);
        }
    }
}