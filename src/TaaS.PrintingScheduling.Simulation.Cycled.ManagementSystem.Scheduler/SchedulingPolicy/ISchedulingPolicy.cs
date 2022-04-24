using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingPolicy
{
    public interface ISchedulingPolicy<TTime> where TTime : struct
    {
        bool IsAllowed(ScheduleOption<TTime> option);
    }
}