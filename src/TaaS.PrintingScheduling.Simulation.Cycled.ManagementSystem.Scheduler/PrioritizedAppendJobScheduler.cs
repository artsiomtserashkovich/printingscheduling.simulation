using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Choosers;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public class PrioritizedAppendJobScheduler<TTime> : IJobScheduler<TTime> where TTime : struct
    {
        private readonly ISchedulingContextFactory<TTime> _contextFactory;
        private readonly IPrioritizedScheduleOptionsQuery<TTime> _optionsQuery;
        private readonly IPrioritizedOptionChooser<TTime> _optionChooser;

        public PrioritizedAppendJobScheduler(
            ISchedulingContextFactory<TTime> contextFactory, 
            IPrioritizedScheduleOptionsQuery<TTime> optionsQuery, 
            IPrioritizedOptionChooser<TTime> optionChooser)
        {
            _contextFactory = contextFactory;
            _optionsQuery = optionsQuery;
            _optionChooser = optionChooser;
        }
        
        public SchedulingResult<TTime> Schedule(
            PrintingJob<TTime> job, 
            IEnumerable<PrinterExecutionState<TTime>> states,
            TTime schedulingTime)
        {
            var printerContexts = states
                .Select(state => _contextFactory.CreateFilledContext(state, schedulingTime))
                .ToArray();
            
            var options = _optionsQuery.GetOptions(printerContexts, job, schedulingTime);
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