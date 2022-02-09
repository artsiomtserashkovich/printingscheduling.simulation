using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator
{
    public interface ISimulationEngine
    {
        IReadOnlyCollection<JobExecutionResult<long>> Simulate();
    }
}