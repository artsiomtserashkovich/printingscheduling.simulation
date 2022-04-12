using FluentAssertions;
using NUnit.Framework;
using TaaS.PrintingScheduling.Simulation.Cycled.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Tests.Simulator.CycledEngine.Context
{
    public class CycledSimulationContextTests
    {
        [Test]
        public void CurrentCycle_HandleFewCycles_ShouldReturnCorrectCycle()
        {
            var sut = new CycledSimulationContext();
            
            sut.NextCycle();
            sut.NextCycle();

            sut.CurrentCycle.Should().Be(2);
        }
    }
}