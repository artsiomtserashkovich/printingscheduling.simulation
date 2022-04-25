using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized.PriorityCalculation.Time;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized.SchedulingPolicy;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized
{
    public class TimeUnscopedPrioritizedOptionsQuery<TTime> : IPrioritizedScheduleOptionsQuery<TTime>
        where TTime : struct
    {
        private readonly IScheduleOptionsQuery<TTime> _optionsQuery;
        private readonly ISchedulingPolicy<TTime> _reschedulingPolicy;
        private readonly IResolutionPriorityCalculator _resolutionPriorityCalculator;
        private readonly ITimePriorityCalculator<TTime> _timePriorityCalculator;

        public TimeUnscopedPrioritizedOptionsQuery(
            IScheduleOptionsQuery<TTime> optionsQuery, 
            ISchedulingPolicy<TTime> reschedulingPolicy,
            IResolutionPriorityCalculator resolutionPriorityCalculator, 
            ITimePriorityCalculator<TTime> timePriorityCalculator)
        {
            _optionsQuery = optionsQuery;
            _reschedulingPolicy = reschedulingPolicy;
            _resolutionPriorityCalculator = resolutionPriorityCalculator;
            _timePriorityCalculator = timePriorityCalculator;
        }

        public IReadOnlyCollection<PrioritizedScheduleOption<TTime>> GetOptions(
            IEnumerable<IPrinterSchedulingState<TTime>> states, 
            PrintingJob<TTime> job,
            TTime schedulingTime)
        {
            var options = _optionsQuery.GetOptions(states, job);

            var minResolution = options.Min(option => option.Printer.Resolution);
            var maxResolution = options.Max(option => option.Printer.Resolution);

            return options
                .Where(_reschedulingPolicy.IsAllowed)
                .Select(option => GetScheduleOption(option, minResolution, maxResolution, schedulingTime))
                .ToArray();
        }

        private PrioritizedScheduleOption<TTime> GetScheduleOption(
            ScheduleOption<TTime> option,
            double minResolution,
            double maxResolution,
            TTime schedulingTime)
        {
            var resolutionPriorityParams = new ResolutionPriorityParameters(
                minResolution, 
                maxResolution, 
                option.Job.Specification.Resolution, 
                option.Printer.Resolution);
            var resolutionPriority = _resolutionPriorityCalculator.Calculate(resolutionPriorityParams);

            var timePriority = _timePriorityCalculator.Calculate(
                option.TimeSlot.Finish,
                schedulingTime,
                option.Job.CommittedFinishTime);

            return new PrioritizedScheduleOption<TTime>(
                option.Printer,
                option.Job,
                option.TimeSlot,
                timePriority,
                resolutionPriority);
        }
    }
}