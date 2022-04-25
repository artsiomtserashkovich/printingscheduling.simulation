using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized.SchedulingPolicy;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized.TimeSlotCalculator;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized;

public class PolicyBasedOptionsQuery<TTime> : IScheduleOptionsQuery<TTime>
    where TTime : struct
{
    private readonly IJobTimeSlotCalculator<TTime> _timeSlotCalculator;
    private readonly ISchedulingPolicy<TTime> _schedulingPolicy;

    public PolicyBasedOptionsQuery(IJobTimeSlotCalculator<TTime> timeSlotCalculator, ISchedulingPolicy<TTime> schedulingPolicy)
    {
        _timeSlotCalculator = timeSlotCalculator;
        _schedulingPolicy = schedulingPolicy;
    }

    public IReadOnlyCollection<ScheduleOption<TTime>> GetOptions(
        IEnumerable<IPrinterSchedulingState<TTime>> states,
        PrintingJob<TTime> job)
    {
        return states
            .Select(state => GetScheduleOption(state, job))
            .Where(option => _schedulingPolicy.IsAllowed(option))
            .ToArray();
    }

    private ScheduleOption<TTime> GetScheduleOption(
        IPrinterSchedulingState<TTime> context,
        PrintingJob<TTime> job)
    {
        var timeSlot = _timeSlotCalculator.Calculate(context.Printer, job.Specification, context.NextAvailableTime);

        return new ScheduleOption<TTime>(context.Printer, job, timeSlot);
    }
}