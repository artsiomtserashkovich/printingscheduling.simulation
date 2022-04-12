using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;

namespace TaaS.PrintingScheduling.Simulation.Cycled.Context
{
    public interface ICycledSimulationContext : IJobResultCollector<long>
    {
        public long CurrentCycle { get; }
    }
}