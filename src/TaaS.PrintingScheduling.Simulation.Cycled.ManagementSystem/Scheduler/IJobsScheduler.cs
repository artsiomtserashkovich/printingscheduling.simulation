using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public interface IJobsScheduler<TTime> where TTime : struct
    {
        public SchedulingResult<TTime> Schedule(
            IEnumerable<JobSpecification<TTime>> incomingJobs, 
            IEnumerable<IPrinterSchedulingState<TTime>> states);
    }
}