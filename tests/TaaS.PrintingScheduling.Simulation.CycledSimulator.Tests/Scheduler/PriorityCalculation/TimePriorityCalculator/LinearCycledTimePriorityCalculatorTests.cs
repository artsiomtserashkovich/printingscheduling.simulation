using System;
using FluentAssertions;
using NUnit.Framework;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Scheduler.PriorityCalculation.TimePriorityCalculator;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Tests.Scheduler.PriorityCalculation.TimePriorityCalculator
{
    public class LinearCycledTimePriorityCalculatorTests
    {
        [TestCase(10, 100, 10, 1)]
        [TestCase(10, 100, 100, 0)]
        [TestCase(10, 100, 55, 0.5)]
        
        [TestCase(10, 110, 80, 0.3)]
        [TestCase(10, 100, 80, 0.222)]
        
        [TestCase(10, 110, 30, 0.8)]
        [TestCase(10, 100, 30, 0.778)]
        public void Calculate_ValidParameters_ShouldCalculateCorrectPriority(
            int minimumCycle, int maximumCycle, int cycle, double expectedResult)
        {
            var sut = new LinearTimePriorityCalculator();
            
            sut.Calculate(cycle, minimumCycle, maximumCycle).Should().Be(expectedResult);
        }
        
        [TestCase(-1, 1, 1)]
        [TestCase(-1, -1, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(0, 1, 1)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 3, 1)]
        [TestCase(1, 3, 4)]
        public void Calculate_InvalidIncomingCycle_ShouldThrowInvalidOperationException(
            int minimumCycle, int maximumCycle, int cycle)
        {
            var sut = new LinearTimePriorityCalculator();
            
            Action action = () => sut.Calculate(cycle, minimumCycle, maximumCycle);

            action.Should().Throw<ArgumentException>();
        }
    }
}