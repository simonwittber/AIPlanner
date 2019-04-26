 AIPlanner provides:
 - A C# DSL for defining a Hierarchial Task Network.
 - A HTN planner which can use the network to create a plan.
 - A runner which will execute the plan.

Define the HTN
--------------
```
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
```
