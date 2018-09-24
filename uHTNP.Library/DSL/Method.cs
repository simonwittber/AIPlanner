using System.Collections.Generic;

namespace uHTNP.DSL
{
    /// <summary>
    /// A Method is a collection of tasks that are grouped by a common list of
    /// preconditions. The parent container the list of Methods is a 
    /// CompoundTask.
    /// </summary>
    public class Method
    {
        public string name = string.Empty;

        internal List<Precondition> preconditions = new List<Precondition>();

        internal List<TaskReference> tasks = new List<TaskReference>();
    }
}