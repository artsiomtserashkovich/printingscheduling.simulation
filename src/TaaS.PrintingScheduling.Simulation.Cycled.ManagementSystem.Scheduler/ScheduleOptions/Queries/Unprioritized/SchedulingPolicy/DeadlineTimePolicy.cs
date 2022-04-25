namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized.SchedulingPolicy;

public class DeadlineTimePolicy: ISchedulingPolicy<long>
{
    private readonly ISchedulingPolicy<long>? _nextPolicy;

    public DeadlineTimePolicy(ISchedulingPolicy<long>? nextPolicy = null)
    {
        _nextPolicy = nextPolicy;
    }
    
    public bool IsAllowed(ScheduleOption<long> option)
    {
        var allowedVolume = option.TimeSlot.Finish < option.Job.CommittedFinishTime;
        
        return allowedVolume && (_nextPolicy?.IsAllowed(option) ?? true);
    }
}