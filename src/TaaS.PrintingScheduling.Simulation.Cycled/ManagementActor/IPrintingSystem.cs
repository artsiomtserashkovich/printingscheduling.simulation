using TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor
{
    public interface IPrintingSystem<TTime> where TTime : struct
    {
        void RegisterFinishedJob(IPrinter printer);

        IPrintingJobExecutable<TTime>? ScheduleNextJob(IPrinter printer);
    }
}