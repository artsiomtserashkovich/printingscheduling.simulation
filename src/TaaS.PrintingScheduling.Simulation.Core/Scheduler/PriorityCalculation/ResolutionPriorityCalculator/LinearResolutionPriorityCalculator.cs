using System;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.PriorityCalculation.ResolutionPriorityCalculator
{
    public class LinearResolutionPriorityCalculator : IResolutionPriorityCalculator
    {
        private const int FloatPartPrecision = 3;
        private const double ExpectedComparingPrecision = 0.01;

        private readonly double _minimumPrinterResolution;
        private readonly double _maximumPrinterResolution;
        private readonly double _expectedResolution;
        private readonly double _thresholdValue;

        public LinearResolutionPriorityCalculator(
            double minimumPrinterResolution, 
            double maximumPrinterResolution,
            double expectedResolution,
            double thresholdValue)
        {
            AssertCtorParameters(minimumPrinterResolution, maximumPrinterResolution, expectedResolution, thresholdValue);
            
            _minimumPrinterResolution = minimumPrinterResolution;
            _maximumPrinterResolution = maximumPrinterResolution;
            _expectedResolution = expectedResolution;
            _thresholdValue = thresholdValue;
        }
        
        public double Calculate(double printerResolution)
        {
            AssertCalculateParameters(printerResolution);

            if (printerResolution > _expectedResolution)
            {
                var priority = 
                    (_maximumPrinterResolution - printerResolution) / (_maximumPrinterResolution - _expectedResolution) 
                    * _thresholdValue;
                
                return Math.Round(priority, FloatPartPrecision);
            }
            else if (Math.Abs(printerResolution - _expectedResolution) < ExpectedComparingPrecision)
            {
                var priority = 1 - _thresholdValue;
                
                return Math.Round(priority, FloatPartPrecision);
            }
            else
            {
                var priority = 
                    ((_expectedResolution - printerResolution) / (_expectedResolution - _minimumPrinterResolution))
                    * _thresholdValue + (1 - _thresholdValue);
                
                return Math.Round(priority, FloatPartPrecision);
            }
        }


        private static void AssertCtorParameters(
            double minimumPrinterResolution,
            double maximumPrinterResolution,
            double expectedResolution,
            double thresholdValue)
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
            
            if (thresholdValue <= 0 || thresholdValue >= 0.5)
            {
                throw new ArgumentException(
                    "Can't be less or equal '0' or more than '0.5'." +
                    $" Current value: '{thresholdValue}'.", 
                    nameof(thresholdValue));
            }
            

            if (minimumPrinterResolution >= maximumPrinterResolution)
            {
                throw new ArgumentException(
                    $"{nameof(minimumPrinterResolution)} can't more or equal {nameof(maximumPrinterResolution)} with value: '{maximumPrinterResolution}'." +
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
        }
        
        private  void AssertCalculateParameters(double printerResolution)
        {
            if (printerResolution < _minimumPrinterResolution)
            {
                throw new InvalidOperationException(
                    $"Can't calculate priority for value that less than {nameof(_minimumPrinterResolution)} " +
                    $"with value: '{_minimumPrinterResolution}'.");
            }

            if (printerResolution > _maximumPrinterResolution)
            {
                throw new InvalidOperationException(
                    $"Can't calculate priority for value that more than {nameof(_maximumPrinterResolution)} "+
                    $"with value: '{_maximumPrinterResolution}'.");
            }
        }
    }
}