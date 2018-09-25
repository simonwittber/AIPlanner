using Xunit;
using AIPlanner.DSL;
using static AIPlanner.DSL.Domain;

namespace AIPlanner
{
    public class ComplexHTNPTests
    {

        [Fact]
        public void TestDomainGraphAnotherPass()
        {
            var d = CreateDomain();
            var state = new WorldState();
            state.Set("PlayerNotVisible", true);
            var plan = HTNPlanner.CreatePlan(state, d);
            Assert.Collection(plan, A => Assert.Equal("WalkToRandomPosition", A.name), A => Assert.Equal("LookAround", A.name));
            var success = false;
            d.BindAction("PlayWalkAnimation", currentState =>
            {
                success = true;
                return ActionState.Success;
            });
            Assert.False(success);
            var runner = new PlanRunner(d, plan);
            var planState = runner.Execute(state);
            Assert.True(success);
            Assert.Equal(PlanState.Completed, planState);
            Assert.False(state.Get("PlayerDead"));

            plan = HTNPlanner.CreatePlan(state, d);
            Assert.Collection(plan, A => Assert.Equal("AttackPlayer", A.name));

            success = false;
            d.BindAction("PlayAttackAnimation", currentState =>
            {
                success = true;
                return ActionState.Success;
            });
            runner = new PlanRunner(d, plan);
            planState = runner.Execute(state);
            Assert.True(success);
            Assert.Equal(PlanState.Completed, planState);
            Assert.True(state.Get("PlayerDead"));

        }

        public Domain CreateDomain()
        {
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
                    .Cost(10)
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
        }



    }
}