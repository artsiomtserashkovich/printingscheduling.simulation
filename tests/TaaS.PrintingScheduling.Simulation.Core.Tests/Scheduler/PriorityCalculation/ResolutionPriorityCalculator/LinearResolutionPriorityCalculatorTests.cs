using System.Collections.Generic;
using NUnit.Framework;

namespace TaaS.PrintingScheduling.Simulation.Core.Tests.Scheduler.PriorityCalculation.ResolutionPriorityCalculator
{
    public class LinearResolutionPriorityCalculatorTests
    {
        
        [TestCaseSource(nameof(GetCalculateTestCases))]
        public void Calculate_ValidParameters_ShouldCalculateCorrectPriority(
            ResolutionPriorityCalculatorOptions options,
            ResolutionPriorityParameters parameters,
            double expectedResult)
        {
            var sut = new LinearResolutionPriorityCalculator(options);
            
            sut.Calculate(parameters).Should().Be(expectedResult);
        }
        
        public static IEnumerable<TestCaseData> GetCalculateTestCases()
        {
            yield return new TestCaseData(
                new ResolutionPriorityCalculatorOptions(0.2),
                new ResolutionPriorityParameters(0.1, 1.1, 0.3, 0.2),
                0.9);
            yield return new TestCaseData(
                new ResolutionPriorityCalculatorOptions(0.2),
                new ResolutionPriorityParameters(0.1, 1.1, 0.5, 0.5),
                0.8);
            
            yield return new TestCaseData(
                new ResolutionPriorityCalculatorOptions(0.2),
                new ResolutionPriorityParameters(0.1, 1.1, 0.5, 0.8),
                0.1);
            
        }

        /*[TestCase(0.0)]
        [TestCase(0.5)]
        public void Ctor_InvalidParameters_ShouldThrowArgumentException(double thresholdValue)
        {
            Action action = () => new LinearResolutionPriorityCalculator(thresholdValue);

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(-0.1, 0.2, 0.15, 0.2)]
        [TestCase(0.1, -0.2, 0.15, 0.2)]
        [TestCase(0.1, 0.2, -0.15, 0.2)]
        [TestCase(0.2, 0.1, 0.15, 0.2)]
        [TestCase(0.1, 0.2, 0.25, 0.2)]
        [TestCase(0.1, 0.2, 0.05, 0.2)]
        [TestCase(0.1, 0.2, 0.15, 0.0)]
        [TestCase(0.1, 0.2, 0.15, 0.5)]
        [TestCase(0.1, 0.2, 0.05)]
        [TestCase(0.1, 0.2, 0.25)]
        public void Calculate_InvalidParameters_ShouldThrowInvalidOperationException(
            double minimumPrinterResolution,
            double maximumPrinterResolution,
            double printerResolution)
        {
            var sut = new LinearResolutionPriorityCalculator(
                minimumPrinterResolution, maximumPrinterResolution, 0.15, 0.15);

            Action action = () => sut.Calculate(printerResolution);

            action.Should().Throw<InvalidOperationException>();
        }*/
    }
}