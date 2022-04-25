using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Choosers;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public class ProfilesBasedJobScheduler<TTime> : IJobScheduler<TTime> where TTime : struct
    {
        private readonly ISchedulingContextFactory<TTime> _contextFactory;
        private readonly IPrioritizedScheduleProfilesQuery<TTime> _profilesQuery;
        private readonly IPrioritizedProfilesChooser<TTime> _profilesChooser;

        public ProfilesBasedJobScheduler(
            ISchedulingContextFactory<TTime> contextFactory,
            IPrioritizedScheduleProfilesQuery<TTime> profilesQuery,
            IPrioritizedProfilesChooser<TTime> profilesChooser)
        {
            _contextFactory = contextFactory;
            _profilesQuery = profilesQuery;
            _profilesChooser = profilesChooser;
        }

        public SchedulingResult<TTime> Schedule(
            PrintingJob<TTime> job,
            IEnumerable<PrinterExecutionState<TTime>> states,
            TTime schedulingTime)
        {
            var previousScheduledJobs = GetPreviousScheduledJobs(states);
            Console.WriteLine($"Time: {DateTime.Now}.Scheduling cycle: {schedulingTime}. jobs to schedule: {previousScheduledJobs.Count}");
            
            var contexts = states
                .Select(state => _contextFactory.CreateEmptyContext(state, schedulingTime))
                .ToArray();

            var profiles = _profilesQuery
                .GetProfiles(contexts, previousScheduledJobs, job, schedulingTime);
            var profile = _profilesChooser.ChooseProfile(profiles);
            if (profile == null)
            {
                return SchedulingResult<TTime>.NotScheduled();
            }

            foreach (var option in profile.Options.OrderBy(o => o.TimeSlot.Finish))
            {
                contexts
                    .Single(context => context.Printer.Id == option.Printer.Id)
                    .ApplySchedule(new JobSchedule<TTime>(option.Job, option.TimeSlot));
            }

            return SchedulingResult<TTime>.Scheduled(
                contexts.ToDictionary(
                    context => context.Printer.Id,
                    context => context.Schedules));
        }

        private IReadOnlyCollection<PrintingJob<TTime>> GetPreviousScheduledJobs(IEnumerable<PrinterExecutionState<TTime>> states)
        {
            return states
                .SelectMany(
                    state => 
                        state.SchedulesQueue?.Select(schedule => schedule.Job) 
                        ?? Array.Empty<PrintingJob<TTime>>())
                .ToArray();
        }
    }
}