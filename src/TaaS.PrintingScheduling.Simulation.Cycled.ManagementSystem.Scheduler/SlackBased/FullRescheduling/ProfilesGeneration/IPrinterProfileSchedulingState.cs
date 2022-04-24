using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ScheduleOption;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration
{
    public interface IPrinterProfileSchedulingState<TTime> : IPrinterSchedulingState<TTime> where TTime : struct
    {
        IReadOnlyCollection<ProfileScheduleOption<TTime>> Schedules { get; }
    }
}

