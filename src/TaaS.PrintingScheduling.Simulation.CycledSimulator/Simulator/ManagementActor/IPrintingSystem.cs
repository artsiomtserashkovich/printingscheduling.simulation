using TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem.Printer;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor
{
    public interface IPrintingSystem
    {
        void RegisterFinishedJob(IPrinter printer, ICycledJob finishedJob, ICycledSimulationContext cycledContext);
        
        ICycledJob? ScheduleNextJob(IPrinter printer, ICycledSimulationContext cycledContext);
    }
}