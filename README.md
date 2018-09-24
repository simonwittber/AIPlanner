 uHTNP provides an DSL for defining a Hierarchial Task Network, which can be used to create a plan, which can then be executed. ##### Example:  This is the DSL used to define a BTN. 

######  code

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
