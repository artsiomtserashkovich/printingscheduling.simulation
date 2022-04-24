using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ScheduleOption
{
    public interface IProfileScheduleOptionsQuery<TTime> where TTime : struct
    {
        IReadOnlyCollection<ProfileScheduleOption<TTime>> GetOptions(
            IEnumerable<IPrinterSchedulingState<TTime>> states, 
            PrintingJob<TTime> job,
            TTime schedulingTime);
    }
}

