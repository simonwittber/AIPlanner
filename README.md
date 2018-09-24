 uHTNP provides:
 - A C# DSL for defining a Hierarchial Task Network.
 - A planner which can use the network to create a plan.
 - A runner which will execute the plan.


```
    using (var domain = Domain.New())
    {
        DefinePrimitiveTask("WalkToRandomPosition")
            .Actions("PlayWalkAnimation");

        DefinePrimitiveTask("LookAround")
            .Actions("PlayLookAnimation")
            .Set("PlayerIsVisible")
            .Unset("PlayerNotVisible");

        DefinePrimitiveTask("AttackPlayer")
            .Conditions("PlayerIsVisible")
            .Actions("PlayAttackAnimation")
            .Set("PlayerIsDead")
            .Set("PlayerNotVisible")
            .Set("PlayerDead")
            .Unset("PlayerIsVisible");


        DefineCompoundTask("BeAnEnemy")
            .DefineMethod("FindPlayer")
                .Conditions("PlayerNotVisible")
                .Tasks("WalkToRandomPosition", "LookAround")
            .DefineMethod("AttackPlayer")
                .Conditions("PlayerIsVisible")
                .Tasks("AttackPlayer");

        SetRootTask("BeAnEnemy");

        return domain;
    }
```
