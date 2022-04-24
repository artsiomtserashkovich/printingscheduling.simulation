using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.LeastFinishTime;

public interface IScheduleOptionChooser<TTime> where TTime : struct
{
    public ScheduleOption<TTime>? ChoseBestOption(
        IReadOnlyCollection<ScheduleOption<TTime>> options);
}