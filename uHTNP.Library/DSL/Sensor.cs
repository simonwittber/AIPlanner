namespace uHTNP.DSL
{
    /// <summary>
    /// A Sensor instance contains a delegate which is passed a WorldState
    /// instance. The delegate can inspect the world and modify the world state
    /// if required.
    /// </summary>
    public class Sensor
    {
        static readonly System.Action<WorldState> DefaultSensor = (A) => { };

        public string name = string.Empty;

        public System.Action<WorldState> sensorDelegate = DefaultSensor;
    }
}