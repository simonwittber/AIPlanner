using Xunit;
using uHTNP;
using uHTNP.DSL;
using static uHTNP.DSL.Domain;

namespace uHTNP
{


    public class SimpleHTNPTests
    {

        public Domain CreateDomain()
        {
            using (var domain = Domain.New())
            {
                DefinePrimitiveTask("P1")
                    .Conditions("P1C1", "P1C2")
                    .Actions("P1A1")
                    .Set("X")
                    .Unset("Y");

                DefinePrimitiveTask("P2")
                    .Actions("P1A2")
                    .Set("X")
                    .Unset("B");

                DefineCompoundTask("C1")
                    .DefineMethod("CT1")
                        .Conditions("C1C1")
                        .Tasks("P1", "P2")
                    .DefineMethod("CT2")
                        .Conditions("C1C2")
                        .Tasks("P1");

                SetRootTask("C1");

                return domain;
            }
        }



        [Fact]
        public void TestPlanner()
        {
            var d = CreateDomain();
            Assert.Equal(d.root, d.tasks["C1"]);
            var state = new WorldState();
            state.Set("C1C2", true);
            var plan = Planner.CreatePlan(state, d);
            Assert.NotNull(plan);
            Assert.Empty(plan);
            state.Set("P1C1", true);
            state.Set("P1C2", true);
            plan = Planner.CreatePlan(state, d);
            Assert.Collection(plan, (A) => Assert.Equal("P1", A.name));
        }

        [Fact]
        public void TestDomainGraph()
        {
            var d = CreateDomain();
            Assert.Contains(d.preconditions, A => A.Key == "P1C1");
            Assert.Contains(d.preconditions, A => A.Key == "P1C2");
            Assert.Contains(d.preconditions, A => A.Key == "C1C1");
            Assert.Contains(d.preconditions, A => A.Key == "C1C2");

            Assert.Contains(d.actions, A => A.Key == "P1A1");
            Assert.Contains(d.actions, A => A.Key == "P1A2");

            Assert.Contains(d.tasks, A => A.Key == "P1");
            Assert.Contains(d.tasks, A => A.Key == "P2");
            Assert.Contains(d.tasks, A => A.Key == "C1");
        }

        [Fact]
        public void TestActionEffects()
        {
            var d = CreateDomain();
            var state = new WorldState();
            state.Set("C1C2", true);
            state.Set("P1C1", true);
            state.Set("P1C2", true);
            var plan = Planner.CreatePlan(state, d);
            var runner = new PlanRunner(d, plan);
            runner.Execute(state);
            Assert.Contains(state.states.Keys, A => A == "X");
            Assert.Contains(state.states.Keys, A => A == "Y");
            Assert.True(state.Get("X"));
            Assert.False(state.Get("Y"));
        }
    }
}