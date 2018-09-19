using System.Collections.Generic;
using System.Linq;

namespace uHTNP.DSL
{
    public class PrimitiveTask : Task
    {
        public Domain domain;
        public List<Precondition> preconditions = new List<Precondition>();
        public Action action = new Action();
        public List<Effect> effects = new List<Effect>();

        public PrimitiveTask Conditions(params string[] preconditions)
        {
            this.preconditions.AddRange(from i in preconditions select domain.GetPrecondition(i));
            return this;
        }

        public PrimitiveTask Actions(string actionName)
        {
            action = domain.GetAction(actionName);
            return this;
        }

        public PrimitiveTask Set(string stateName)
        {
            effects.Add(new Effect { name = stateName, value = true });
            return this;
        }

        public PrimitiveTask Unset(string stateName)
        {
            effects.Add(new Effect { name = stateName, value = false });
            return this;
        }
    }
}