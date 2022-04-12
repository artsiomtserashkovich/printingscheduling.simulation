using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.IncomingJobsQueue
{
    public class CycledIncomingJobsQueue : IIncomingJobsQueue
    {
        private readonly PriorityQueue<JobSpecification<long>, long> _jobs;

        public CycledIncomingJobsQueue(PriorityQueue<JobSpecification<long>, long> jobs)
        {
            _jobs = jobs;
        }

        public bool IsContainsJobs => _jobs.Count != 0;
        
        public IReadOnlyCollection<JobSpecification<long>> Dequeue(ICycledSimulationContext cycledContext)
        {
            var batch = new List<JobSpecification<long>>();
            while (IsContainsJobs && _jobs.Peek().IncomingTime <= cycledContext.CurrentCycle)
            {
                batch.Add(_jobs.Dequeue());
            }
            
            return batch;
        }
    }
}