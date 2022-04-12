using System;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution
{
    public class LinearResolutionPriorityCalculator : IResolutionPriorityCalculator
    {
        private readonly ResolutionPriorityCalculatorOptions _options;
        
        public LinearResolutionPriorityCalculator(ResolutionPriorityCalculatorOptions options)
        {
            _options = options;
        }
        
        public double Calculate(ResolutionPriorityParameters parameters)
        {
            return Math.Round(CalculateExactValue(parameters), _options.CalculatedValuePrecision);
        }

        private double CalculateExactValue(ResolutionPriorityParameters parameters)
        {
            var resolutionDeviation  = parameters.PrinterResolution - parameters.ExpectedResolution;
            if (Math.Abs(resolutionDeviation) < _options.DeviationFromExpectedThreshold)
            {
                return 1 - _options.EdgeValuesThresholdStep;
            }

            return resolutionDeviation switch
            {
                > 0 => CalculateAboveExpected(parameters),
                < 0 => CalculateBelowExpected(parameters)
            };
        }

        private double CalculateAboveExpected(ResolutionPriorityParameters parameters)
        {
            return 
                (parameters.MaximumPrinterResolution - parameters.PrinterResolution) 
                / (parameters.MaximumPrinterResolution - parameters.ExpectedResolution) 
                * _options.EdgeValuesThresholdStep;
        }

        private double CalculateBelowExpected(ResolutionPriorityParameters parameters)
        {
            return 
                (parameters.ExpectedResolution - parameters.PrinterResolution) 
                / (parameters.ExpectedResolution - parameters.MinimumPrinterResolution)
                * _options.EdgeValuesThresholdStep 
                + (1 - _options.EdgeValuesThresholdStep);
        }
    }
}