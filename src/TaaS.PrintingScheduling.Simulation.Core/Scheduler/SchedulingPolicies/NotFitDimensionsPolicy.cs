using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingPolicies
{
    public class NotFitDimensionsPolicy<TTime> : ISchedulingPolicy<TTime> where TTime : struct 
    {
        private readonly ISchedulingPolicy<TTime> _nextPolicy;

        public NotFitDimensionsPolicy(ISchedulingPolicy<TTime> nextPolicy = null)
        {
            _nextPolicy = nextPolicy;
        }

        public bool IsAllowed(PrinterSpecification printer, JobSpecification<TTime> job)
        {
            var allowedVolume = printer.PrintingDimension > job.Dimension || 
                printer.PrintingDimension == job.Dimension;

            return allowedVolume && (_nextPolicy?.IsAllowed(printer, job) ?? true);
        }
    }
}