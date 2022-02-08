using System;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Scheduler
{
    public class CycledTimeSlatCalculator : IJobTimeSlotCalculator<long>
    {
        public ExecutionTimeSlot<long> Calculate(
            PrinterSpecification printer, JobSpecification<long> job, long lastJobFinishTime)
        {
            var startTime = lastJobFinishTime + 1;
            
            var jobVolume = job.Dimension.Volume;
            var printingVolumePerCycle = printer.Resolution * printer.Resolution * printer.PrintingSpeed;
            var jobCyclesDuration = (long)Math.Ceiling(jobVolume / printingVolumePerCycle);

            return new ExecutionTimeSlot<long>(startTime, startTime + jobCyclesDuration - 1);
        }
    }
}