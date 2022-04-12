using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.PrintingResult
{
    public interface IJobResultCollector<TTime> where TTime : struct
    {
        void RegisterResult(JobExecutionResult<TTime> result);
    }
}