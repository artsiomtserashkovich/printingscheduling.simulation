using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler
{
    public interface IJobTimeSlotCalculator<TTime> where TTime : struct
    {
        TimeSlot<TTime> Calculate(
            PrinterSpecification printer, JobSpecification<TTime> job, TTime lastJobFinishTime);
    }
}