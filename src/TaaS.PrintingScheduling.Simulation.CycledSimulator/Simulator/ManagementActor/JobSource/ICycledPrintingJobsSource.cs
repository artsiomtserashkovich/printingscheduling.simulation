using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.JobSource
{
    public interface ICycledPrintingJobsSource
    {
        public bool IsContainsJobs { get; } 
        
        IReadOnlyCollection<JobSpecification<long>> GetIncomingJobs(
            ICycledSimulationContext cycledContext);
    }
}