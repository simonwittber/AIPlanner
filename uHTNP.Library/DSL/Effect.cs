namespace uHTNP.DSL
{
    /// <summary>
    /// An Effect is a state name and a value. It is used by PrimitiveTasks to
    /// store the changes (effects) that are applied to world state when the
    /// task is executed.
    /// </summary>
    public class Effect
    {
        /// <summary>
        /// The name of the state.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// The value of the state.
        /// </summary>
        public bool value;
    }
}