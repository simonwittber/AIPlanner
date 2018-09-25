namespace AIPlanner.DSL
{
    /// <summary>
    /// A Sensor instance contains a delegate which is passed a WorldState
    /// instance. The delegate can inspect the world and modify the world state
    /// if required.
    /// </summary>
    public class Sensor
    {
        static readonly System.Action<WorldState> DefaultSensor = (A) => { };

        /// <summary>
        /// The unique name of the sensor.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// The sensor delegate which performs logic tests and sets world state
        /// to suit.
        /// </summary>
        public System.Action<WorldState> sensorDelegate = DefaultSensor;
    }
}