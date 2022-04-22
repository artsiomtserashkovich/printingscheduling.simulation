using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulingContext
{
    public interface ISchedulingContextFactory<TTime> where TTime : struct
    {
        public PrinterSchedulingContext<TTime> CreateFilledContext(PrinterExecutionState<TTime> state, TTime schedulingTime);
        
        public PrinterSchedulingContext<TTime> CreateEmptySchedulesContext(PrinterExecutionState<TTime> state);
    }
}