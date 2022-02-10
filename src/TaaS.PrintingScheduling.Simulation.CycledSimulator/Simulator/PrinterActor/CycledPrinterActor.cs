using System;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.Jobs;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor
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

        public bool IsFree => _currentJob == null;

        public int Id => _specification.Id; 

        public void ExecuteCycle(ICycledSimulationContext cycledSimulationContext)
        {
            if (_currentJob?.IsComplete ?? false)
            {
                var result = _currentJob.GetResultReport(_specification);
                _printingSystem.RegisterFinishedJob(this, result);
            }
            if (_currentJob == null || _currentJob.IsComplete)
            {
                InitializeNewJob();
            }

            ExecuteCycleImplementation(cycledSimulationContext);
        }

        private void ExecuteCycleImplementation(ICycledSimulationContext cycledSimulationContext)
        {
            _currentJob?.Execute(_specification, cycledSimulationContext);
        }

        private void InitializeNewJob()
        {
            var newIncomingJob = _printingSystem.ScheduleNextJob(this);
            AssertIncomingJob(newIncomingJob);
            
            _currentJob = newIncomingJob;
        }

        private void AssertIncomingJob(ICycledJob? job)
        {
            if (job != null && job.Specification.Dimension > _specification.PrintingDimension)
            {
                throw new InvalidOperationException(
                    $@"Dimension of the job with id='{job.Specification.Id}' is more then printer dimension.");
            }
        }
    }
}