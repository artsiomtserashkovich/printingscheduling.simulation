using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;

namespace TaaS.PrintingScheduling.Simulation.Cycled.Engine
{
    public interface ISimulationEngine
    {
        IReadOnlyCollection<JobExecutionResult<long>> Simulate();
    }
}