using System;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs
{
    public class CycledJob : ICycledJob
    {
        private double _remainingVolume;

        public CycledJob(JobSpecification<long> specification)
        {
            Specification = specification;

            _remainingVolume = specification.Dimension.Volume;
        }

        public bool IsComplete => _remainingVolume == 0;

        public JobSpecification<long> Specification { get; }
        
        public long? ExecutionStartTime { get; private set; }
        
        public long? ExecutionFinishTime { get; private set; }

        public void Execute(PrinterSpecification printer, ICycledSimulationContext cycledContext)
        {
            if (IsComplete)
            {
                throw new InvalidOperationException("Job completed.");
            }
            if (ExecutionStartTime is null)
            {
                ExecutionStartTime = cycledContext.CurrentCycle;
            }

            var printingVolumePerCycle = printer.Resolution * printer.Resolution * printer.PrintingSpeed;
            
            var newRemainingVolume =_remainingVolume - printingVolumePerCycle;
            if (newRemainingVolume <= 0)
            {
                ExecutionFinishTime = cycledContext.CurrentCycle;
                newRemainingVolume = 0;
            }
            
            _remainingVolume = newRemainingVolume;
        }
    }
}