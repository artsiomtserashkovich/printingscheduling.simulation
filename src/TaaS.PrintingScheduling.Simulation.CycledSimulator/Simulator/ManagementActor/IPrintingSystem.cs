using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.Jobs;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor
{
    public interface IPrintingSystem
    {
        void RegisterFinishedJob(JobExecutionResult<long> jobResult);
        
        ICycledJob? ScheduleNextJob(IPrinter printer);
    }
}