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

    Domain CreateDomain()
    {
        fuel.value = 13;

        using (domain = Domain.New())
        {
            DefineWorldState(health, fuel, speed);

            DefinePrimitiveTask(MoveSomewhereRandom)
                .Conditions(fuel > 1)
                .Set(fuel - 1, speed + 1);

            DefinePrimitiveTask(PlayAttackAnimation)
                .Set(health + 1);

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
        if (HTNPlanner.CreatePlan(domain))
            runner = new PlanRunner(domain);
    }

    void Update()
    {
        if (runner != null && domain.planState == PlanState.InProgress || domain.planState == PlanState.Waiting)
        {
            domain.planState = runner.Execute();
        }
    }
}
