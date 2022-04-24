using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.IncomingJobsQueue
{
    public interface IIncomingJobsQueue
    {
        public bool IsContainsJobs { get; } 
        
        public PrintingJob<long>? Dequeue(ICycledSimulationContext cycledContext);
    }
}