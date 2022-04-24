using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased;

public interface IExecutionStatesJobsExtractor<TTime> where TTime : struct
{
    public IReadOnlyCollection<PrintingJob<TTime>> Extract(IEnumerable<PrinterExecutionState<TTime>> states);
}