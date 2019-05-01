using static AIPlanner.Domain;
using UnityEngine;
using AIPlanner;


public class ExampleMonoBehaviour : MonoBehaviour
{
    public Domain domain;

    public StateVariable health;
    public StateVariable fuel;
    public StateVariable speed;

    PlanRunner runner;
    Plan plan = new Plan();

    WorldState worldState;

    Domain CreateDomain()
    {
        fuel.value = 13;

        using (domain = Domain.New())
        {
            worldState = DefineWorldState(health, fuel, speed);

            DefinePrimitiveTask(MoveSomewhereRandom)
                .Conditions(fuel > 1)
                .Effects(fuel - 1, speed + 1);

            DefinePrimitiveTask(PlayAttackAnimation)
                .Effects(health + 1);

            DefineCompoundTask("Main")
                .DefineMethod("FindPlayer")
                    .Conditions(health > 1, fuel > 10)
                    .Tasks(MoveSomewhereRandom)
                .DefineMethod("AttackPlayer")
                    .Conditions(health == 3)
                    .Tasks(PlayAttackAnimation);

            SetRootTask("Main");
            return domain;
        }
    }

    ActionState MoveSomewhereRandom()
    {
        return ActionState.InProgress;
    }

    ActionState PlayAttackAnimation()
    {
        return ActionState.Success;
    }

    ActionState PlayLookAnimation()
    {
        return ActionState.Success;
    }


    void Start()
    {

        domain = CreateDomain();
        // if (HTNPlanner.CreatePlan(domain, worldState, plan))
        //     runner = new PlanRunner(plan);
    }

    void Update()
    {
        if (runner != null && runner.PlanState == PlanState.InProgress || runner.PlanState == PlanState.Waiting)
        {
            runner.Execute(worldState);
        }
    }
}
