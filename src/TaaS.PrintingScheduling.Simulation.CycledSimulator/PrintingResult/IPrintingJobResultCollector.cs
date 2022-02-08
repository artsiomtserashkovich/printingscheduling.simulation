using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.PrintingResult
{
    public interface IPrintingJobResultCollector
    {
        void RegisterCompletedJob(ICycledJob completedJob, ICycledSimulationContext simulationContext);
    }
}