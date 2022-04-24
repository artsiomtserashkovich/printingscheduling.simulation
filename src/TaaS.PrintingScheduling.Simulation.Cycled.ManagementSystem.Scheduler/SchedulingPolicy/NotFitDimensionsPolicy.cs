using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingPolicy
{
    public class NotFitDimensionsPolicy<TTime> : ISchedulingPolicy<TTime> where TTime : struct 
    {
        private readonly ISchedulingPolicy<TTime>? _nextPolicy;

        public NotFitDimensionsPolicy(ISchedulingPolicy<TTime>? nextPolicy = null)
        {
            _nextPolicy = nextPolicy;
        }
        
        public bool IsAllowed(ScheduleOption<TTime> option)
        {
            var allowedVolume = option.Printer.PrintingDimension > option.Job.Specification.Dimension || 
                option.Printer.PrintingDimension == option.Job.Specification.Dimension;

            return allowedVolume && (_nextPolicy?.IsAllowed(option) ?? true);
        }
    }
}