using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration
{
    public interface IPrioritizedScheduleProfilesQuery<TTime> where TTime : struct
    {
        public IReadOnlyCollection<PrioritizedSchedulesProfile<TTime>> GetProfiles(
            IEnumerable<IPrinterSchedulingState<TTime>> currentStates,
            IEnumerable<PrintingJob<TTime>> jobs,
            TTime schedulingTime);
    } 
}