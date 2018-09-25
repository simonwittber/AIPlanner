using System;

namespace AIPlanner.DSL
{
    /// <summary>
    /// A Precondition maps a a state that must be satisfied, as well as an
    /// optional func which must return true for the condition to succeed.
    /// </summary>
    public class Precondition
    {
        /// <summary>
        /// The name of the state.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// The required value of that state.
        /// </summary>
        public bool value = true;

        /// <summary>
        /// The optional procedural precondition that can return true to satisfy
        /// the precondition.
        /// </summary>
        public Func<bool> proceduralPrecondition;
    }
}