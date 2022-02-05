using TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.CycledEngine;
using TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem.Printer;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem.PrintingResult
{
    public interface IPrintingJobResultCollector
    {
        void RegisterCompletedJob(ICycledJob completedJob, ICycledSimulationContext simulationContext);
    }
}