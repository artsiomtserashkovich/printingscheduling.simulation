using System;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.Jobs;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;

namespace TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor
{
    public class CycledPrinterActor : IPrinter, ICycledPrinterActor
    {
        private readonly IPrintingSystem _printingSystem;
        private readonly PrinterSpecification _specification;

        private ICycledJob? _currentJob = null;
        
        public CycledPrinterActor(
            PrinterSpecification specification,
            IPrintingSystem printingSystem)
        {
            _specification = specification;
            _printingSystem = printingSystem;
        }

        public int Id => _specification.Id;
        
        private bool IsFree => _currentJob == null;

        public void ExecuteCycle()
        {
            if (!IsFree && _currentJob.IsComplete)
            {
                _printingSystem.RegisterFinishedJob(this);
                _currentJob = null;
            }
            
            if (IsFree)
            {
                var newIncomingJob = _printingSystem.ScheduleNextJob(this);
            
                AssertIncomingJob(newIncomingJob);
                _currentJob = newIncomingJob;
            }

            _currentJob?.Execute(_specification);
        }

        private void AssertIncomingJob(ICycledJob? job)
        {
            if (job != null && job.Specification.Dimension > _specification.PrintingDimension)
            {
                _currentJob = null;
                throw new InvalidOperationException(
                    $"Dimension of the job with id='{job.Specification.Id}' is more then printer dimension.");
            }
        }
    }
}