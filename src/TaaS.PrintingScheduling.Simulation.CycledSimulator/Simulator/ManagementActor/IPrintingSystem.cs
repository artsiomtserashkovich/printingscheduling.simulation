using TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem.Printer;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor
{
    public interface IPrintingSystem
    {
        void RegisterFinishedJob(ICycledJob finishedJob);
        
        ICycledJob ScheduleNextJob(IPrinter printer);
    }
}