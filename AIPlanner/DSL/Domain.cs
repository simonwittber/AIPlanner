using System;
using System.Collections.Generic;


namespace AIPlanner
{
    public delegate ActionState ActionDelegate();

    /// <summary>
    /// The Domain is a collection of tasks, conditions and actions that are 
    /// used to create a plan based on world state. 
    /// </summary>
    [System.Serializable]
    public class Domain : IDisposable
    {

        public List<PrimitiveTask> plan = new List<PrimitiveTask>();
        public PlanState planState = PlanState.Waiting;

        internal Dictionary<string, Task> tasks = new Dictionary<string, Task>();

        internal Task root;

        internal StateVariable[] worldState;

        [ThreadStatic] static Domain active;

        public override string ToString()
        {
            return root.ToString();
        }

        /// <summary>
        /// Define the collection of StateVariable instances that define the world state for this domain.
        /// </summary>
        /// <param name="variables"></param>
        public static void DefineWorldState(params StateVariable[] variables)
        {
            CheckInternalState();
            for (var i = 0; i < variables.Length; i++)
            {
                variables[i].index = i;
            }
            active.worldState = variables;
        }

        /// <summary>
        /// Create a new domain and set it to be the active domain. The domain
        /// contains all tasks and sensors.
        /// </summary>
        /// <returns>The new.</returns>
        public static Domain New()
        {
            if (active != null) throw new Exception("Must dispose current domain before calling New.");
            active = new Domain();
            return active;
        }

        /// <summary>
        /// Defines a named compound task and adds it to the active domain.
        /// A compound task contains methods which contain conditions and
        /// other primitive and compound tasks.
        /// </summary>
        /// <returns>The compound task.</returns>
        /// <param name="name">Name.</param>
        public static CompoundTask DefineCompoundTask(string name)
        {
            CheckInternalState();
            var t = new CompoundTask { name = name, domain = active };
            active.tasks.Add(name, t);
            return t;
        }

        /// <summary>
        /// Defines a named primitive task and adds it to the active domain.
        /// A primitive task checks conditions, runs an action, and applies an
        /// effect to the world state.
        /// </summary>
        /// <returns>The primitive task.</returns>
        /// <param name="name">Name.</param>
        public static PrimitiveTask DefinePrimitiveTask(ActionDelegate fn)
        {
            CheckInternalState();
            var name = fn.Method.Name;
            var p = new PrimitiveTask { name = name, domain = active };
            p.action = fn;
            active.tasks.Add(name, p);
            return p;
        }

        /// <summary>
        /// Sets the root task of the planner. This is the compound task which 
        /// is executed first.
        /// </summary>
        /// <param name="name">Name.</param>
        public static void SetRootTask(string name)
        {
            CheckInternalState();
            active.root = active.tasks[name];
        }

        /// <summary>
        /// Closes the domain, sets active domain to null.
        /// </summary>
        public void Dispose()
        {
            active = null;
        }

        static void CheckInternalState()
        {
            if (active == null) throw new Exception("Must call Domain.New before using DSL methods.");
        }
    }
}