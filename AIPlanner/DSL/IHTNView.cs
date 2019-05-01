namespace AIPlanner
{
    public interface IHTNView
    {
        Domain Domain { get; }
        PlanRunner PlanRunner { get; }
        Plan Plan { get; }
    }
}