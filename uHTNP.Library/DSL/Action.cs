using System.Collections.Generic;

namespace uHTNP.DSL
{
    public class Action
    {
        static readonly System.Func<WorldState, ActionState> DefaultAction = (A) => ActionState.Success;

        public string name = string.Empty;
        public System.Func<WorldState, ActionState> actionDelegate = DefaultAction;
    }
}