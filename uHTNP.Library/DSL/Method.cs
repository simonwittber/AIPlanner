using System.Collections.Generic;

namespace uHTNP.DSL
{
    public class Method
    {
        public string name = string.Empty;
        internal List<Precondition> preconditions = new List<Precondition>();
        internal List<TaskReference> tasks = new List<TaskReference>();
    }
}