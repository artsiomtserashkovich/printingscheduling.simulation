using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace TaaS.PrintingScheduling.Simulation.Core.Tests
{
    public class DimensionTests
    {
        [TestCase(30.5, 40, -1)] 
        [TestCase(0, 20.1, 30)]
        [TestCase(14, -10, 40)]
        public void Ctor_InvalidParameters_ShouldThrowArgumentException(
            double x, double y, double z)
        {
            Action action = () => new Dimension(x, y, z);

            action.Should().Throw<ArgumentException>();
        }

        [TestCaseSource(nameof(GetOperatorEqualCases))]
        public void OperatorEqual_ProvideValues_ShouldCompareNestedValues(
            Dimension left, Dimension right, bool expectedResult)
        {
            (left == right).Should().Be(expectedResult);
        }
        
        [TestCaseSource(nameof(GetOperatorLessCases))]
        public void OperatorLess_ProvideValues_ShouldCompareNestedValues(
            Dimension left, Dimension right, bool expectedResult)
        {
            (left < right).Should().Be(expectedResult);
        }
        
        [TestCaseSource(nameof(GetOperatorMoreCases))]
        public void OperatorMore_ProvideValues_ShouldCompareNestedValues(
            Dimension left, Dimension right, bool expectedResult)
        {
            (left > right).Should().Be(expectedResult);
        }

        public static IEnumerable<TestCaseData> GetOperatorEqualCases()
        {
            yield return new TestCaseData(
                new Dimension(10, 20, 33.3), 
                new Dimension(10, 20, 33.3), 
                true);
            yield return new TestCaseData(
                new Dimension(10, 20, 33.3), 
                new Dimension(20, 10, 33.3), 
                false);
        }
        
        public static IEnumerable<TestCaseData> GetOperatorLessCases()
        {
            yield return new TestCaseData(
                new Dimension(10, 20, 33.3), 
                new Dimension(10, 22, 31.3), 
                false);
            yield return new TestCaseData(
                new Dimension(10, 20, 33.3), 
                new Dimension(20, 21, 33.3), 
                true);
            yield return new TestCaseData(
                new Dimension(10, 21, 33.3), 
                new Dimension(10, 21, 33.3), 
                false);
        }
        
        public static IEnumerable<TestCaseData> GetOperatorMoreCases()
        {
            yield return new TestCaseData(
                new Dimension(10, 22, 33.3), 
                new Dimension(10, 22, 31.3), 
                true);
            yield return new TestCaseData(
                new Dimension(10, 22, 32.3), 
                new Dimension(20, 20, 33.3), 
                false);
            yield return new TestCaseData(
                new Dimension(10, 22, 33.3), 
                new Dimension(10, 22, 33.3), 
                false);
        }
    }
}