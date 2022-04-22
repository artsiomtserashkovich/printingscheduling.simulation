using System;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution
{
    public class ResolutionPriorityParameters
    {
        public double MinimumPrinterResolution { get; }
        
        public double MaximumPrinterResolution { get; }
        
        public double ExpectedResolution { get; }
        
        public double PrinterResolution { get; }

        public ResolutionPriorityParameters(
            double minimumPrinterResolution, 
            double maximumPrinterResolution,
            double expectedResolution,
            double printerResolution)
        {
            MinimumPrinterResolution = minimumPrinterResolution;
            MaximumPrinterResolution = maximumPrinterResolution;
            ExpectedResolution = expectedResolution;
            PrinterResolution = printerResolution;

            Validate();
        }

        private void Validate()
        {
            if (MinimumPrinterResolution <= 0)
            {
                throw new ArgumentException(
                    "Can't be 0 or negative." +
                    $" Current value: '{MinimumPrinterResolution}'.", 
                    nameof(MinimumPrinterResolution));
            }

            if (MaximumPrinterResolution <= 0)
            {
                throw new ArgumentException(
                    "Can't be 0 or negative." +
                    $" Current value: '{MaximumPrinterResolution}'.", 
                    nameof(MaximumPrinterResolution));
            }

            if (ExpectedResolution <= 0)
            {
                throw new ArgumentException(
                    "Can't be 0 or negative." +
                    $" Current value: '{ExpectedResolution}'.", 
                    nameof(ExpectedResolution));
            }
            
            if (MinimumPrinterResolution > MaximumPrinterResolution)
            {
                throw new ArgumentException(
                    $"{nameof(MinimumPrinterResolution)} can't be more {nameof(MaximumPrinterResolution)} with value: '{MaximumPrinterResolution}'." +
                    $" Current value: '{MinimumPrinterResolution}'.",
                    nameof(MinimumPrinterResolution));
            }

            if (ExpectedResolution < MinimumPrinterResolution || ExpectedResolution > MaximumPrinterResolution)
            {
                throw new ArgumentException(
                    $"{nameof(ExpectedResolution)} can't more than {nameof(MaximumPrinterResolution)} with value: '{MaximumPrinterResolution}'" +
                    $"or less than {nameof(MinimumPrinterResolution)} with value: '{MinimumPrinterResolution}'." +
                    $" Current value: '{MinimumPrinterResolution}'.",
                    nameof(ExpectedResolution));
            }
            
            if (PrinterResolution < MinimumPrinterResolution)
            {
                throw new InvalidOperationException(
                    $"Can't calculate priority for value that less than {nameof(MinimumPrinterResolution)} " +
                    $"with value: '{MinimumPrinterResolution}'.");
            }

            if (PrinterResolution > MaximumPrinterResolution)
            {
                throw new InvalidOperationException(
                    $"Can't calculate priority for value that more than {nameof(MaximumPrinterResolution)} "+
                    $"with value: '{MaximumPrinterResolution}'.");
            }
        }
    }
}