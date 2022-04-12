using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.SchedulingPolicy
{
    public class WorseResolutionPolicy<TTime> : ISchedulingPolicy<TTime> where TTime : struct
    {
        private readonly ISchedulingPolicy<TTime> _nextPolicy;

        public WorseResolutionPolicy(ISchedulingPolicy<TTime> nextPolicy = null)
        {
            _nextPolicy = nextPolicy;
        }
        
        public bool IsAllowed(PrinterSpecification printer, JobSpecification<TTime> job)
        {
            var allowedResolution = printer.Resolution <= job.Resolution;
            
            return allowedResolution && (_nextPolicy?.IsAllowed(printer, job) ?? true);
        }
    }
}