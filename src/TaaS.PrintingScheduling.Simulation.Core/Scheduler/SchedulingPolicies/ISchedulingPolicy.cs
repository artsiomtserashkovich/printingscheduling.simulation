using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingPolicies
{
    public interface ISchedulingPolicy<TTime> where TTime : struct
    {
        bool IsAllowed(PrinterSpecification printer, JobSpecification<TTime> job);
    }
}