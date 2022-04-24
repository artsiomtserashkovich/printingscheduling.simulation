using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext
{
    public interface IPrinterSchedulingState<out TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public TTime NextAvailableTime { get; }
    }
}