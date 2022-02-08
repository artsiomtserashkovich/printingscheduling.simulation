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
            var jobDuration = (long)Math.Ceiling(job.Volume / printer.PrintingSpeed);

            return new ExecutionTimeSlot<long>(startTime, startTime + jobDuration);
        }
    }
}