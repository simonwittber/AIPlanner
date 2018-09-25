 uHTNP provides:
 - A C# DSL for defining a Hierarchial Task Network.
 - A planner which can use the network to create a plan.
 - A GIAP planner which can use the primitive tasks in the netwrok to create a plan.
 - A runner which will execute the plan.

Define the BTN
--------------
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

Bind Methods
------------
This connects plan execution into your application.
Similar Bind methods exist for procedural preconditions and sensors.
```
    var d = CreateDomain();
    d.BindAction("PlayWalkAnimation", currentState =>
    {
        return ActionState.Success;
    });
```

Create a Plan
-------------
```
    var state = new WorldState();
    state.Set("PlayerNotVisible", true);
    var plan = Planner.CreatePlan(state, d);
```

Execute the Plan
----------------
```
    var runner = new PlanRunner(d, plan);
    var planState = runner.Execute(state);
```
