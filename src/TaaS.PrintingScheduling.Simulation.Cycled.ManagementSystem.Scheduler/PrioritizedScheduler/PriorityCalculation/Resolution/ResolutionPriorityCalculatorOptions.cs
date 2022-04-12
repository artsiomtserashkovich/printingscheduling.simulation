using System;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution
{
    public class ResolutionPriorityCalculatorOptions
    {
        public double EdgeValuesThresholdStep { get; }
        
        public int CalculatedValuePrecision { get; }

        public double DeviationFromExpectedThreshold { get; }
        
        public ResolutionPriorityCalculatorOptions(
            double edgeValuesThresholdStep,
            int calculatedValuePrecision = 3,
            double deviationFromExpectedThreshold = 0.01)
        {
            if (edgeValuesThresholdStep is <= 0 or >= 0.5)
            {
                throw new ArgumentException(
                    $"Can't be less or equal '0' or more than '0.5'. Current value: '{edgeValuesThresholdStep}'.", 
                    nameof(edgeValuesThresholdStep));
            }

            if (calculatedValuePrecision <= 0)
            {
                throw new ArgumentException(
                    $"Can't be less or equal '0'. Current value: '{calculatedValuePrecision}'.", 
                    nameof(calculatedValuePrecision));
            }
            
            if (deviationFromExpectedThreshold is <= 0 or >= 1)
            {
                throw new ArgumentException(
                    $"Can't be less or equal '0' or more than '1'. Current value: '{deviationFromExpectedThreshold}'.", 
                    nameof(deviationFromExpectedThreshold));
            }
            
            EdgeValuesThresholdStep = edgeValuesThresholdStep;
            CalculatedValuePrecision = calculatedValuePrecision;
            DeviationFromExpectedThreshold = deviationFromExpectedThreshold;
        }
    }
}