using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.TimeAndResolutionPrioritized
{
    public interface IPrioritizedScheduleOptionChooser<TTime> where TTime : struct
    {
        public PrioritizedScheduleOption<TTime>? ChoseBestOption(
            IReadOnlyCollection<PrioritizedScheduleOption<TTime>> options);
    }
}