namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized.SchedulingPolicy
{
    public interface ISchedulingPolicy<TTime> where TTime : struct
    {
        bool IsAllowed(ScheduleOption<TTime> option);
    }
}