using System;

namespace AIPlanner
{

    public class Condition
    {
        public StateVariable variable;
        public System.Func<StateVariable, bool> fn;

        public bool IsTrue(StateVariable[] state)
        {
            return fn(state[variable.index]);
        }
    }
}