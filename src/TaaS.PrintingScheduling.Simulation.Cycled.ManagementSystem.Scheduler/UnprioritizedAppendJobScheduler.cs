using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Choosers;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public class UnprioritizedAppendJobScheduler<TTime> : IJobScheduler<TTime>
        where TTime : struct
    {
        private readonly IScheduleOptionsQuery<TTime> _optionsQuery;
        private readonly IOptionChooser<TTime> _optionChooser;
        private readonly ISchedulingContextFactory<TTime> _contextFactory;

        public UnprioritizedAppendJobScheduler(
            IScheduleOptionsQuery<TTime> optionsQuery,
            IOptionChooser<TTime> optionChooser,
            ISchedulingContextFactory<TTime> contextFactory)
        {
            _optionsQuery = optionsQuery;
            _optionChooser = optionChooser;
            _contextFactory = contextFactory;
        }

        public SchedulingResult<TTime> Schedule(
            PrintingJob<TTime> job,
            IEnumerable<PrinterExecutionState<TTime>> states,
            TTime schedulingTime)
        {
            var printerContexts = states
                .Select(state => _contextFactory.CreateFilledContext(state, schedulingTime))
                .ToArray();

            var options = _optionsQuery.GetOptions(printerContexts, job);
            var option = _optionChooser.ChooseOption(options);
            if (option is null)
            {
                return SchedulingResult<TTime>.NotScheduled();
            }
            
            var changedContext = printerContexts.First(context => context.Printer.Id == option.Printer.Id);
            changedContext.ApplySchedule(new JobSchedule<TTime>(option.Job, option.TimeSlot));
            
            return SchedulingResult<TTime>.Scheduled(
                new Dictionary<int, IReadOnlySchedulesQueue<TTime>>
                {
                    [changedContext.Printer.Id] = changedContext.Schedules
                });
        }
    }
}