using System.Collections.Generic;

namespace TaaS.PrintingScheduling.Simulation.Core.PrintingResult
{
    public class InMemoryResultsCollector<TTime> : IJobResultCollector<TTime>, IJobResultStorage<TTime> where TTime : struct
    {
        private readonly List<JobExecutionResult<TTime>> _results = new();
        
        public void RegisterResult(JobExecutionResult<TTime> result)
        {
            _results.Add(result);
        }

        public IReadOnlyCollection<JobExecutionResult<TTime>> GetResults()
        {
            return _results;
        }
    }
}