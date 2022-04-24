using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.TimeAndResolutionPrioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulesQuery
{
    public interface IPrioritizedScheduleOptionsQuery<TTime> where TTime : struct
    {
        public IReadOnlyCollection<PrioritizedScheduleOption<TTime>> GetOptions(
            IEnumerable<IPrinterSchedulingState<TTime>> states,
            PrintingJob<TTime> job);
    }
}