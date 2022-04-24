using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.LeastFinishTime;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.TimeAndResolutionPrioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Time;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulesQuery
{
    public class TimeAndResolutionPriorityOptionsQuery<TTime> : IPrioritizedScheduleOptionsQuery<TTime> 
        where TTime : struct
    {
        private readonly IScheduleOptionsQuery<TTime> _optionsQuery;
        private readonly ITimePriorityCalculator<TTime> _timePriorityCalculator;
        private readonly IResolutionPriorityCalculator _resolutionPriorityCalculator;

        public TimeAndResolutionPriorityOptionsQuery(
            IScheduleOptionsQuery<TTime> optionsQuery,
            
            ITimePriorityCalculator<TTime> timePriorityCalculator, 
            IResolutionPriorityCalculator resolutionPriorityCalculator)
        {
            _optionsQuery = optionsQuery;
            _timePriorityCalculator = timePriorityCalculator;
            _resolutionPriorityCalculator = resolutionPriorityCalculator;
        }

        public IReadOnlyCollection<PrioritizedScheduleOption<TTime>> GetOptions(
            IEnumerable<IPrinterSchedulingState<TTime>> states, 
            PrintingJob<TTime> job)
        {
            var options = _optionsQuery.GetOptions(states, job);

            var minResolution = options.Min(option => option.Printer.Resolution);
            var maxResolution = options.Max(option => option.Printer.Resolution);
            
            var minFinishTime = options.Min(option => option.TimeSlot.Finish);
            var maxFinishTime = options.Max(option => option.TimeSlot.Finish);

            return options
                .Select(schedule => 
                    CalculatePriority(schedule, minFinishTime, maxFinishTime, minResolution, maxResolution))
                .ToArray();
        }

        private PrioritizedScheduleOption<TTime> CalculatePriority(
            ScheduleOption<TTime> option,
            TTime minFinishTime, 
            TTime maxFinishTime, 
            double minResolution, 
            double maxResolution)
        {
            var timePriority = _timePriorityCalculator
                .Calculate(option.TimeSlot.Finish, minFinishTime, maxFinishTime);

            var calculatorParameters = new ResolutionPriorityParameters(
                minResolution,
                maxResolution,
                option.Job.Specification.Resolution,
                option.Printer.Resolution);
            
            var resolutionPriority = _resolutionPriorityCalculator.Calculate(calculatorParameters);

            return new PrioritizedScheduleOption<TTime>(
                option.Printer,
                option.Job,
                option.TimeSlot,
                timePriority, 
                resolutionPriority);
        }
        
    }
}