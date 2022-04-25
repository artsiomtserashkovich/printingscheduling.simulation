using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries
{
    public interface IPrioritizedScheduleProfilesQuery<TTime> where TTime : struct
    {
        public IReadOnlyCollection<IPrioritizedOptionsProfile<TTime>> GetProfiles(
            IEnumerable<IPrinterSchedulingState<TTime>> baseStates,
            IEnumerable<PrintingJob<TTime>> previousJobs,
            PrintingJob<TTime> job,
            TTime schedulingTime);
    } 
}