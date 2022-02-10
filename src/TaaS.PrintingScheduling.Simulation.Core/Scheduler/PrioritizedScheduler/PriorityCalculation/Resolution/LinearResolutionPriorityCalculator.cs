using System;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution
{
    public class LinearResolutionPriorityCalculator : IResolutionPriorityCalculator
    {
        private const int FloatPartPrecision = 3;
        private const double ExpectedComparingPrecision = 0.01;

        private readonly double _thresholdValue;

        public LinearResolutionPriorityCalculator(
            
            double thresholdValue)
        {
            if (thresholdValue <= 0 || thresholdValue >= 0.5)
            {
                throw new ArgumentException(
                    "Can't be less or equal '0' or more than '0.5'." +
                    $" Current value: '{thresholdValue}'.", 
                    nameof(thresholdValue));
            }
            
            _thresholdValue = thresholdValue;
        }
        
        public double Calculate(
            double minimumPrinterResolution, 
            double maximumPrinterResolution,
            double expectedResolution,
            double printerResolution)
        {
            AssertCalculateParameters(minimumPrinterResolution, maximumPrinterResolution, expectedResolution, printerResolution);

            if (printerResolution > expectedResolution)
            {
                var priority = 
                    (maximumPrinterResolution - printerResolution) / (maximumPrinterResolution - expectedResolution) 
                    * _thresholdValue;
                
                return Math.Round(priority, FloatPartPrecision);
            }
            else if (Math.Abs(printerResolution - expectedResolution) < ExpectedComparingPrecision)
            {
                var priority = 1 - _thresholdValue;
                
                return Math.Round(priority, FloatPartPrecision);
            }
            else
            {
                var priority = 
                    ((expectedResolution - printerResolution) / (expectedResolution - minimumPrinterResolution))
                    * _thresholdValue + (1 - _thresholdValue);
                
                return Math.Round(priority, FloatPartPrecision);
            }
        }
        
        private  void AssertCalculateParameters(
            double minimumPrinterResolution,
            double maximumPrinterResolution,
            double expectedResolution,
            double printerResolution)
        {
            if (minimumPrinterResolution <= 0)
            {
                throw new ArgumentException(
                    "Can't be 0 or negative." +
                    $" Current value: '{minimumPrinterResolution}'.", 
                    nameof(minimumPrinterResolution));
            }

            if (maximumPrinterResolution <= 0)
            {
                throw new ArgumentException(
                    "Can't be 0 or negative." +
                    $" Current value: '{maximumPrinterResolution}'.", 
                    nameof(maximumPrinterResolution));
            }

            if (expectedResolution <= 0)
            {
                throw new ArgumentException(
                    "Can't be 0 or negative." +
                    $" Current value: '{expectedResolution}'.", 
                    nameof(expectedResolution));
            }
            
            if (minimumPrinterResolution > maximumPrinterResolution)
            {
                throw new ArgumentException(
                    $"{nameof(minimumPrinterResolution)} can't be more {nameof(maximumPrinterResolution)} with value: '{maximumPrinterResolution}'." +
                    $" Current value: '{minimumPrinterResolution}'.",
                    nameof(minimumPrinterResolution));
            }

            if (expectedResolution < minimumPrinterResolution || expectedResolution > maximumPrinterResolution)
            {
                throw new ArgumentException(
                    $"{nameof(expectedResolution)} can't more than {nameof(maximumPrinterResolution)} with value: '{maximumPrinterResolution}'" +
                    $"or less than {nameof(minimumPrinterResolution)} with value: '{minimumPrinterResolution}'." +
                    $" Current value: '{minimumPrinterResolution}'.",
                    nameof(expectedResolution));
            }
            
            
            if (printerResolution < minimumPrinterResolution)
            {
                throw new InvalidOperationException(
                    $"Can't calculate priority for value that less than {nameof(minimumPrinterResolution)} " +
                    $"with value: '{minimumPrinterResolution}'.");
            }

            if (printerResolution > maximumPrinterResolution)
            {
                throw new InvalidOperationException(
                    $"Can't calculate priority for value that more than {nameof(maximumPrinterResolution)} "+
                    $"with value: '{maximumPrinterResolution}'.");
            }
        }
    }
}