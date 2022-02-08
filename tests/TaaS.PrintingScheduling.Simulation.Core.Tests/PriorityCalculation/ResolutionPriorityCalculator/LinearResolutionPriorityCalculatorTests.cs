using System;
using FluentAssertions;
using NUnit.Framework;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PriorityCalculation.ResolutionPriorityCalculator;

namespace TaaS.PrintingScheduling.Simulation.Core.Tests.PriorityCalculation.ResolutionPriorityCalculator
{
    public class LinearResolutionPriorityCalculatorTests
    {
        [TestCase(
            0.1, 
            1.1, 
            0.5, 
            0.2,
            0.3,
            0.9)]
        [TestCase(
            0.1, 
            1.1, 
            0.5, 
            0.2,
            0.5,
            0.8)]
        [TestCase(
            0.1, 
            1.1, 
            0.5, 
            0.2,
            0.8,
            0.1)]
        public void Calculate_ValidParameters_ShouldCalculateCorrectPriority(
            double minimumPrinterResolution, 
            double maximumPrinterResolution, 
            double expectedResolution, 
            double thresholdValue,
            double printerResolution,
            double expectedPriority)
        {
            var sut = new LinearResolutionPriorityCalculator(
                minimumPrinterResolution, 
                maximumPrinterResolution, 
                expectedResolution, 
                thresholdValue);
            
            sut.Calculate(printerResolution).Should().Be(expectedPriority);
        }
        
        
        [TestCase(-0.1, 0.2, 0.15, 0.2)]
        [TestCase(0.1, -0.2, 0.15, 0.2)]
        [TestCase(0.1, 0.2, -0.15, 0.2)]
        [TestCase(0.2, 0.1, 0.15, 0.2)]
        [TestCase(0.1, 0.2, 0.25, 0.2)]
        [TestCase(0.1, 0.2, 0.05, 0.2)]
        [TestCase(0.1, 0.2, 0.15, 0.0)]
        [TestCase(0.1, 0.2, 0.15, 0.5)]
        public void Ctor_InvalidParameters_ShouldThrowArgumentException(
            double minimumPrinterResolution, 
            double maximumPrinterResolution, 
            double expectedResolution, 
            double thresholdValue)
        {
            Action action = () => new LinearResolutionPriorityCalculator(
                minimumPrinterResolution, 
                maximumPrinterResolution, 
                expectedResolution, 
                thresholdValue);

            action.Should().Throw<ArgumentException>();
        }
        
        [TestCase(0.1, 0.2, 0.05)]
        [TestCase(0.1, 0.2, 0.25)]
        public void Calculate_InvalidIncomingCycle_ShouldThrowInvalidOperationException(
            double minimumPrinterResolution, 
            double maximumPrinterResolution,
            double printerResolution)
        {
            var sut = new LinearResolutionPriorityCalculator(
                minimumPrinterResolution, maximumPrinterResolution, 0.15, 0.15);
            
            Action action = () => sut.Calculate(printerResolution);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}