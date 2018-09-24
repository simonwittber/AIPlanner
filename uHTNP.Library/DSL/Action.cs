using System.Collections.Generic;

namespace uHTNP.DSL
{
    /// <summary>
    /// An Action is analogous to a method call. It is passed the current world
    /// state and must return an ActionState which specifies if the action is
    /// finished, failed, or needs to be called again.
    /// </summary>
    public class Action
    {
        static readonly System.Func<WorldState, ActionState> DefaultAction = (A) => ActionState.Success;

        /// <summary>
        /// The unique name of the action.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// The action delegate which performs the actual work.
        /// </summary>
        public System.Func<WorldState, ActionState> actionDelegate = DefaultAction;
    }
}