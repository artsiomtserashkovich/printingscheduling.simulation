using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased
{
    public class ExecutionStatesJobsExtractor<TTime> : IExecutionStatesJobsExtractor<TTime> where TTime : struct
    {
        public IReadOnlyCollection<PrintingJob<TTime>> Extract(IEnumerable<PrinterExecutionState<TTime>> states)
        {
            return states
                .SelectMany(
                    state => 
                        state.SchedulesQueue?.Select(schedule => schedule.Job) 
                        ?? Array.Empty<PrintingJob<TTime>>())
                .ToArray();
        }
    } 
}