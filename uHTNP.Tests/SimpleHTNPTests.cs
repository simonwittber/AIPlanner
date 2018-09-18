using Xunit;
using uHTNP;
using uHTNP.DSL;
using static uHTNP.DSL.Domain;

namespace uHTNP.Tests
{


    public class SimpleHTNPTests
    {

        public Domain CreateDomain()
        {
            using (var domain = Domain.New())
            {
                DefinePrimitive("P1")
                    .Conditions("P1C1", "P1C2")
                    .Actions("P1A1", "P1A2")
                    .Set("X")
                    .Unset("Y");

                DefinePrimitive("P2")
                    .Actions("P1A1", "P2A2")
                    .Set("X")
                    .Unset("B");

                DefineCompound("C1")
                    .Conditions("C1C1")
                        .Tasks("P1", "P2")
                    .Conditions("C1C2")
                        .Tasks("P1");

                SetRootTask("C1");
                return domain;
            }
        }



        [Fact]
        public void SimpleHTNPTestsSimplePasses()
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

    }
}