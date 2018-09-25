namespace AIPlanner.DSL
{
    /// <summary>
    /// A Procedural cost is a runtime calculated value.
    /// </summary>
    public class ProceduralCost
    {
        static readonly System.Func<WorldState, float> DefaultCost = (A) => 0;

        /// <summary>
        /// The unique name of the cost.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// The costdelegate which performs the actual calculate.
        /// </summary>
        public System.Func<WorldState, float> costDelegate = DefaultCost;
    }
}