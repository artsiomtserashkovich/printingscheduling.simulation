using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.JobSource
{
    public class CycledIncomingJobsSource : ICycledPrintingJobsSource
    {
        private readonly PriorityQueue<JobSpecification<long>, long> _jobs;

        public CycledIncomingJobsSource(PriorityQueue<JobSpecification<long>, long> jobs)
        {
            _jobs = jobs;
        }

        public bool IsContainsJobs => _jobs.Count != 0;
        
        public IReadOnlyCollection<JobSpecification<long>> GetIncomingJobs(ICycledSimulationContext cycledContext)
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