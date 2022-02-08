using System;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PriorityCalculation.TimePriorityCalculator;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Scheduler.PriorityCalculation.TimePriorityCalculator
{
    public class LinearTimePriorityCalculator : ITimePriorityCalculator<int>
    {
        private const int FloatPartPrecision = 3;
        
        private readonly int _minimumCycle;
        private readonly int _maximumCycle;

        public LinearTimePriorityCalculator(int minimumCycle, int maximumCycle)
        {
            AssertCtorParameters(minimumCycle, maximumCycle);
            
            _minimumCycle = minimumCycle;
            _maximumCycle = maximumCycle;
        }
        
        public double Calculate(int cycle)
        {
            AssertCalculateParameters(cycle);

            var result = 1 - (double) (cycle - _minimumCycle) / (_maximumCycle - _minimumCycle);
            
            return Math.Round(result, FloatPartPrecision);
        }
        
        private static void AssertCtorParameters(int minimumCycle, int maximumCycle)
        {
            if (minimumCycle <= 0)
            {
                throw new ArgumentException("Can't be 0 or negative.", nameof(minimumCycle));
            }
            
            if (maximumCycle <= 0)
            {
                throw new ArgumentException("Can't be 0 or negative.", nameof(maximumCycle));
            }
            
            if (minimumCycle >= maximumCycle)
            {
                throw new ArgumentException(
                    $"{nameof(minimumCycle)} can't more or equal {nameof(maximumCycle)} with value: '{maximumCycle}'." +
                    $" Current value: '{minimumCycle}'.",
                    nameof(minimumCycle));
            }
        }
        
        private void AssertCalculateParameters(int cycle)
        {
            if (cycle < _minimumCycle)
            {
                throw new InvalidOperationException(
                    $"Can't calculate priority for value that less than {nameof(_minimumCycle)} " +
                    $"with value: '{_minimumCycle}'.");
            }

            if (cycle > _maximumCycle)
            {
                throw new InvalidOperationException(
                    $"Can't calculate priority for value that more than {nameof(_maximumCycle)} "+
                    $"with value: '{_maximumCycle}'.");
            }
        }
    }
}