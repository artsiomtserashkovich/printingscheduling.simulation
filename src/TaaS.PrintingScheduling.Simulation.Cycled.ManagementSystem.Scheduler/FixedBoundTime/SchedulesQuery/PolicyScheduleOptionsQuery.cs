using TaaS.PrintingScheduling.Simulation.Core.SchedulingPolicy;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulingContext;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.TimeSlotCalculator;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.LeastFinishTime;

public class PolicyScheduleOptionsQuery<TTime> : IScheduleOptionsQuery<TTime>
    where TTime : struct
{
    private readonly IJobTimeSlotCalculator<TTime> _timeSlotCalculator;
    private readonly ISchedulingPolicy<TTime> _schedulingPolicy;

    public PolicyScheduleOptionsQuery(IJobTimeSlotCalculator<TTime> timeSlotCalculator, ISchedulingPolicy<TTime> schedulingPolicy)
    {
        _timeSlotCalculator = timeSlotCalculator;
        _schedulingPolicy = schedulingPolicy;
    }

    public IReadOnlyCollection<ScheduleOption<TTime>> GetOptions(
        IEnumerable<IPrinterSchedulingState<TTime>> states,
        JobSpecification<TTime> job)
    {
        return states
            .Where(state => _schedulingPolicy.IsAllowed(state.Printer, job))
            .Select(state => GetScheduleOption(state, job))
            .ToArray();
    }

    private ScheduleOption<TTime> GetScheduleOption(
        IPrinterSchedulingState<TTime> context,
        JobSpecification<TTime> job)
    {
        var timeSlot = _timeSlotCalculator.Calculate(context.Printer, job, context.NextAvailableTime);

        return new ScheduleOption<TTime>(context.Printer, job, timeSlot);
    }
}