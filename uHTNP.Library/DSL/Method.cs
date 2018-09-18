using System.Collections.Generic;

namespace uHTNP.DSL
{
    public class Method
    {
        public string name = string.Empty;
        public List<Precondition> preconditions = new List<Precondition>();
        public List<TaskReference> tasks = new List<TaskReference>();
    }
}