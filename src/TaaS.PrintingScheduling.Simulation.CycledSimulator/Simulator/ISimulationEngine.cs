using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator
{
    public interface ISimulationEngine
    {
        IReadOnlyCollection<JobExecutionResult<long>> Simulate();
    }
}