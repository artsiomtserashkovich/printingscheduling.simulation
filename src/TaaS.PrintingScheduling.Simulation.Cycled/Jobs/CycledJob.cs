using System;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.Jobs
{
    public class CycledJob : ICycledJob
    {
        private double _remainingVolume;

        public CycledJob(JobSpecification<long> specification)
        {
            Specification = specification;
            _remainingVolume = Specification.Dimension.Volume;
        }

        public bool IsComplete => _remainingVolume == 0;

        public JobSpecification<long> Specification { get; }
        
        public void Execute(PrinterSpecification printer)
        {
            if (IsComplete)
            {
                throw new InvalidOperationException(
                    $"Job can't be executed cause it completed. Job id: '{Specification.Id}'.");
            }
            
            var newRemainingVolume =
                _remainingVolume - CalculateCyclePrintingVolume(printer.Resolution, printer.PrintingSpeed);

            _remainingVolume = newRemainingVolume <= 0 ? 0 : newRemainingVolume;
        }

        private double CalculateCyclePrintingVolume(double resolution, double printingSpeed)
        {
            return printingSpeed * (resolution * resolution) * 10;
        }
    }
}