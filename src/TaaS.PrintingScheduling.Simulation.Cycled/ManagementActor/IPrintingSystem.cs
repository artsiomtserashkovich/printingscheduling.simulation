using TaaS.PrintingScheduling.Simulation.Cycled.Jobs;
using TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor
{
    public interface IPrintingSystem
    {
        void RegisterFinishedJob(IPrinter printer);
        
        ICycledJob? ScheduleNextJob(IPrinter printer);
    }
}