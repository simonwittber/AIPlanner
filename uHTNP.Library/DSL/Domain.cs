using System;
using System.Collections.Generic;


namespace uHTNP.DSL
{
    /// <summary>
    /// The Domain is a collection of tasks, conditions and actions that are 
    /// used to create a plan based on world state. While domain instances do
    /// not share state, the methods they are bound to (using Bind* calls) may,
    /// so it is safer to create a new Domain instance for each actor.
    /// </summary>
    public class Domain : IDisposable
    {
        internal Dictionary<string, Task> tasks = new Dictionary<string, Task>();
        internal Dictionary<string, Precondition> preconditions = new Dictionary<string, Precondition>();
        internal Dictionary<string, Action> actions = new Dictionary<string, Action>();
        internal Dictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();

        internal Task root;

        [ThreadStatic] static Domain active;

        /// <summary>
        /// Bind an action delegate to a named action.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="actionDelegate">Action delegate.</param>
        public void BindAction(string name, System.Func<WorldState, ActionState> actionDelegate)
        {
            actions[name].actionDelegate = actionDelegate;
        }

        /// <summary>
        /// Bind a precondition delgate to a named precondition.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="conditionDelegate">Condition delegate.</param>
        public void BindPrecondition(string name, System.Func<bool> conditionDelegate)
        {
            preconditions[name].proceduralPrecondition = conditionDelegate;
        }

        /// <summary>
        /// Bind a sensor delegate to a named sensor.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="sensorDelegate">Sensor delegate.</param>
        public void BindSensor(string name, Action<WorldState> sensorDelegate)
        {
            sensors[name].sensorDelegate = sensorDelegate;
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
        public static PrimitiveTask DefinePrimitiveTask(string name)
        {
            CheckInternalState();
            var p = new PrimitiveTask { name = name, domain = active };
            active.tasks.Add(name, p);
            return p;
        }

        /// <summary>
        /// Defines a named sensor. A sensor is checked when needed and updates
        /// the world state.
        /// </summary>
        /// <returns>The sensor.</returns>
        /// <param name="name">Name.</param>
        public static Sensor DefineSensor(string name)
        {
            CheckInternalState();
            var s = new Sensor { name = name };
            active.sensors.Add(s.name, s);
            return s;
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
        /// Updates the state of the world by checking all sensors.
        /// </summary>
        /// <param name="currentState">Current state.</param>
        public void UpdateWorldState(WorldState currentState)
        {
            foreach (var s in sensors.Values)
                s.sensorDelegate.Invoke(currentState);
        }

        /// <summary>
        /// Closes the domain, sets active domain to null.
        /// </summary>
        public void Dispose()
        {
            active = null;
        }

        internal Precondition GetPrecondition(string name)
        {
            if (!preconditions.TryGetValue(name, out Precondition precondition))
                precondition = preconditions[name] = new Precondition { name = name };
            return precondition;
        }

        internal Action GetAction(string name)
        {
            if (!actions.TryGetValue(name, out Action action))
                action = actions[name] = new Action { name = name };
            return action;
        }

        static void CheckInternalState()
        {
            if (active == null) throw new Exception("Must call Domain.New before using DSL methods.");
        }
    }
}