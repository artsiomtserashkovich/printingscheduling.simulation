using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased
{
    public class FullReschedulingJobScheduler<TTime> : IJobScheduler<TTime> where TTime : struct
    {
        private readonly ISchedulingContextFactory<TTime> _contextFactory;
        private readonly IExecutionStatesJobsExtractor<TTime> _jobsExtractor;
        private readonly IPrioritizedScheduleProfilesQuery<TTime> _profilesQuery;
        private readonly IPrioritizedProfilesChooser<TTime> _profilesChooser;

        public FullReschedulingJobScheduler(
            ISchedulingContextFactory<TTime> contextFactory,
            IExecutionStatesJobsExtractor<TTime> jobsExtractor,
            IPrioritizedScheduleProfilesQuery<TTime> profilesQuery, 
            IPrioritizedProfilesChooser<TTime> profilesChooser)
        {
            _contextFactory = contextFactory;
            _jobsExtractor = jobsExtractor;
            _profilesQuery = profilesQuery;
            _profilesChooser = profilesChooser;
        }

        public SchedulingResult<TTime> Schedule(
            PrintingJob<TTime> job, 
            IEnumerable<PrinterExecutionState<TTime>> states, 
            TTime schedulingTime)
        {
            var contexts = states
                .Select(state => _contextFactory.CreateEmptyContext(state, schedulingTime))
                .ToArray();

            var jobs = _jobsExtractor.Extract(states).Concat(new [] { job });
            
            Console.WriteLine($"Time: {DateTime.Now}.Scheduling cycle: {schedulingTime}. jobs to schedule: {jobs.Count()}");
            
            var profiles = _profilesQuery.GetProfiles(contexts, jobs, schedulingTime);
            var profile = _profilesChooser.Chose(profiles);
            if (profile == null)
            {
                return SchedulingResult<TTime>.NotScheduled();
            }

            foreach (var state in profile.States)
            {
                var printerContext = contexts
                    .Single(context => context.Printer.Id == state.Printer.Id);

                foreach (var scheduleOption in state.Schedules)
                {
                    printerContext.ApplySchedule(new JobSchedule<TTime>(scheduleOption.Job, scheduleOption.TimeSlot));
                }
            }

            return SchedulingResult<TTime>.Scheduled(
                contexts.ToDictionary(
                    context => context.Printer.Id,
                    context => context.Schedules));
        }
    }
}