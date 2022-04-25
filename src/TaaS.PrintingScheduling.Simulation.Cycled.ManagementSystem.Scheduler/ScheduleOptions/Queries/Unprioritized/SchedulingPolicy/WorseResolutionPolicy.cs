namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized.SchedulingPolicy
{
    public class WorseResolutionPolicy<TTime> : ISchedulingPolicy<TTime> where TTime : struct
    {
        private readonly ISchedulingPolicy<TTime>? _nextPolicy;

        public WorseResolutionPolicy(ISchedulingPolicy<TTime>? nextPolicy = null)
        {
            _nextPolicy = nextPolicy;
        }
        
        public bool IsAllowed(ScheduleOption<TTime> option)
        {
            var allowedResolution = option.Printer.Resolution <= option.Job.Specification.Resolution;
            
            return allowedResolution && (_nextPolicy?.IsAllowed(option) ?? true);
        }
    }
}