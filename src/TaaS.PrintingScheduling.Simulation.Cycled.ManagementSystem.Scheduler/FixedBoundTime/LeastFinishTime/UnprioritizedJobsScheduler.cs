using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.LeastFinishTime
{
    public class UnprioritizedJobsScheduler<TTime> : IJobsScheduler<TTime>
        where TTime : struct
    {
        private readonly IScheduleOptionsQuery<TTime> _optionsQuery;
        private readonly IScheduleOptionChooser<TTime> _optionChooser;
        private readonly ISchedulingContextFactory<TTime> _contextFactory;

        public UnprioritizedJobsScheduler(
            IScheduleOptionsQuery<TTime> optionsQuery,
            IScheduleOptionChooser<TTime> optionChooser,
            ISchedulingContextFactory<TTime> contextFactory)
        {
            _optionsQuery = optionsQuery;
            _optionChooser = optionChooser;
            _contextFactory = contextFactory;
        }

        public SchedulingResult<TTime> Schedule(
            IEnumerable<JobSpecification<TTime>> incomingJobs,
            IEnumerable<PrinterExecutionState<TTime>> states,
            TTime schedulingTime)
        {
            var printerContexts = states
                .Select(state => _contextFactory.CreateFilledContext(state, schedulingTime))
                .ToArray();
            
            var unscheduledJobs = new List<JobSpecification<TTime>>();

            foreach (var incomingJob in incomingJobs)
            {
                var options = _optionsQuery.GetOptions(printerContexts, incomingJob);
                var option = _optionChooser.ChoseBestOption(options);

                if (option is null)
                {
                    unscheduledJobs.Add(incomingJob);
                }
                else
                {
                    printerContexts
                        .First(context => context.Printer.Id == option.Value.Printer.Id)
                        .ApplySchedule(new JobSchedule<TTime>(option.Value.Job, option.Value.TimeSlot));
                }
            }

            return new SchedulingResult<TTime>(
                printerContexts.ToDictionary(
                    context => context.Printer.Id,
                    context => context.Schedules),
                unscheduledJobs);
        }
    }
}