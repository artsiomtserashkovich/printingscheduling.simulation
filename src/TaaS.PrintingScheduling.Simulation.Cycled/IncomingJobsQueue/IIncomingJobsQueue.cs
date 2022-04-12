using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.IncomingJobsQueue
{
    public interface IIncomingJobsQueue
    {
        public bool IsContainsJobs { get; } 
        
        public IReadOnlyCollection<JobSpecification<long>> Dequeue(
            ICycledSimulationContext cycledContext);
    }
}