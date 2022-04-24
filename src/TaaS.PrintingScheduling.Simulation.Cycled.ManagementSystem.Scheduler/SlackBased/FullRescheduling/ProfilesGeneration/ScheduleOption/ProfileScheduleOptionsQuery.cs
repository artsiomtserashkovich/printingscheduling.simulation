using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulesQuery;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Time;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingPolicy;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ScheduleOption
{
    public class ProfileScheduleOptionsQuery<TTime> : IProfileScheduleOptionsQuery<TTime>
        where TTime : struct
    {
        private readonly IScheduleOptionsQuery<TTime> _optionsQuery;
        private readonly ISchedulingPolicy<TTime> _reschedulingPolicy;
        private readonly IResolutionPriorityCalculator _resolutionPriorityCalculator;
        private readonly ITimePriorityCalculator<TTime> _timePriorityCalculator;

        public ProfileScheduleOptionsQuery(
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

        public IReadOnlyCollection<ProfileScheduleOption<TTime>> GetOptions(
            IEnumerable<IPrinterSchedulingState<TTime>> states, 
            PrintingJob<TTime> job,
            TTime schedulingTime)
        {
            var options = _optionsQuery.GetOptions(states, job);

            var minResolution = options.Min(option => option.Printer.Resolution);
            var maxResolution = options.Max(option => option.Printer.Resolution);

            return options
                .Where(_reschedulingPolicy.IsAllowed)
                .Select(option => GetProfileScheduleOption(option, minResolution, maxResolution, schedulingTime))
                .ToArray();
        }

        private ProfileScheduleOption<TTime> GetProfileScheduleOption(
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

            return new ProfileScheduleOption<TTime>(
                option.Printer,
                option.Job,
                option.TimeSlot,
                timePriority,
                resolutionPriority);
        }
    }
}