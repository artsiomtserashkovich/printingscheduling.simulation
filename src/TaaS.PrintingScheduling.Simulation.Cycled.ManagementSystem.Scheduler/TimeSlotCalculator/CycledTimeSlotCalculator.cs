using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.TimeSlotCalculator
{
    public class CycledTimeSlotCalculator : IJobTimeSlotCalculator<long>
    {
        public TimeSlot<long> Calculate(PrinterSpecification printer, JobSpecification<long> job, long startTime)
        {
            var jobVolume = job.Dimension.Volume;
            var resolutionFactor = printer.Resolution * printer.Resolution * 10;
            
            var printingVolumePerCycle = resolutionFactor * printer.PrintingSpeed;
            
            var jobCyclesDuration = (long)Math.Ceiling(jobVolume / printingVolumePerCycle);

            return new TimeSlot<long>(startTime, startTime + jobCyclesDuration);
        }
    }
}