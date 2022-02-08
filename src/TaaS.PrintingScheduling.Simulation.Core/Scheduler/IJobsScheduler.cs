using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler
{
    public interface IJobsScheduler<TTime> where TTime : struct
    {
        public void Schedule(
                IEnumerable<JobSpecification<TTime>> incomingJobs, 
                IEnumerable<IPrinterSchedulingState<TTime>> currentState,
                TTime currentTime);
    }
}