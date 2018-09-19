namespace uHTNP.DSL
{
    public class Sensor
    {
        static readonly System.Action<WorldState> DefaultSensor = (A) => { };

        public string name = string.Empty;
        public System.Action<WorldState> sensorDelegate = DefaultSensor;
    }
}