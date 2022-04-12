using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;

namespace TaaS.PrintingScheduling.Simulation.Cycled.Context
{
    public class CycledSimulationContext : ICycledSimulationContext
    {
        private readonly List<JobExecutionResult<long>> _results = new();
        
        public long CurrentCycle { get; private set; }

        public CycledSimulationContext()
        {
            CurrentCycle = 0;
        }

        public void NextCycle()
        {
            CurrentCycle += 1;
        }

        public IReadOnlyCollection<JobExecutionResult<long>> GetResults() => _results;

        public void RegisterResult(JobExecutionResult<long> result)
        {
            _results.Add(result);
        }
    }
}