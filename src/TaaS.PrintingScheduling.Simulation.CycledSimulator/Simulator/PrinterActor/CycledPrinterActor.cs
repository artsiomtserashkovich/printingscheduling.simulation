using System;
using TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem.Printer;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor
{
    public class CycledPrinterActor : IPrinter, ICycledPrinterActor
    {
        private readonly IPrintingSystem _printingSystem;
        private readonly PrinterSpecification _specification;

        private ICycledJob? _cycledJob = null;
        
        public CycledPrinterActor(
            PrinterSpecification specification,
            IPrintingSystem printingSystem)
        {
            _specification = specification;
            _printingSystem = printingSystem;
        }

        public bool IsFree => _cycledJob == null;

        public int Id => _specification.Id; 

        public void ExecuteCycle(ICycledSimulationContext cycledSimulationContext)
        {
            if (_cycledJob?.IsComplete ?? true)
            {
                if (_cycledJob != null)
                {
                    _printingSystem.RegisterFinishedJob(this, _cycledJob, cycledSimulationContext);
                }
                
                _cycledJob = GetNewScheduledJob(cycledSimulationContext);
            }
            
            _cycledJob?.Execute(_specification, cycledSimulationContext);
        }

        private ICycledJob? GetNewScheduledJob(ICycledSimulationContext cycledSimulationContext)
        {
            var newIncomingJob = _printingSystem.ScheduleNextJob(this, cycledSimulationContext);
            if (newIncomingJob == null)
            {
                return null;
            }
            
            if (newIncomingJob.Specification.Dimension > _specification.PrintingDimension)
            {
                throw new InvalidOperationException(
                    $@"Dimension of the job with id='{newIncomingJob.Specification.Id}' is more then printer dimension.");
            }

            return newIncomingJob;
        }
    }
}