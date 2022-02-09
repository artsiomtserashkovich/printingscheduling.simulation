using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.JobSource
{
    public interface ICycledPrintingJobsSource
    {
        public bool IsContainsJobs { get; } 
        
        public IReadOnlyCollection<JobSpecification<long>> GetIncomingJobs(
            ICycledSimulationContext cycledContext);
    }
}