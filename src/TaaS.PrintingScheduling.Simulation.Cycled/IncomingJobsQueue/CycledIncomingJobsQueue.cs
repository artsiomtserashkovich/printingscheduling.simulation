using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.IncomingJobsQueue
{
    public class CycledIncomingJobsQueue : IIncomingJobsQueue
    {
        // days * hrs * min * sec
        private const long MaxTimeInSystem = 8 * 24 * 60 * 60;
        private readonly PriorityQueue<JobSpecification<long>, long> _jobs;

        public CycledIncomingJobsQueue(PriorityQueue<JobSpecification<long>, long> jobs)
        {
            _jobs = jobs;
        }

        public bool IsContainsJobs => _jobs.Count != 0;
        
        public PrintingJob<long>? Dequeue(ICycledSimulationContext cycledContext)
        {
            if (_jobs.Count != 0 && _jobs.Peek().IncomingTime <= cycledContext.CurrentCycle)
            {
                var job = _jobs.Dequeue();
                return new PrintingJob<long>(job, job.IncomingTime + MaxTimeInSystem);
            }

            return null;
        }
    }
}