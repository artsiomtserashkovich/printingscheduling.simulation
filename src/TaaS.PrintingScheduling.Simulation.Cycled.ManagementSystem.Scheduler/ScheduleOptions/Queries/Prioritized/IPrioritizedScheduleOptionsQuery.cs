using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized
{
    public interface IPrioritizedScheduleOptionsQuery<TTime> where TTime : struct
    {
        IReadOnlyCollection<PrioritizedScheduleOption<TTime>> GetOptions(
            IEnumerable<IPrinterSchedulingState<TTime>> states, 
            PrintingJob<TTime> job,
            TTime schedulingTime);
    }
}

