using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext
{
    public interface ISchedulingContextFactory<TTime> where TTime : struct
    {
        public PrinterSchedulingContext<TTime> CreateFilledContext(
            PrinterExecutionState<TTime> state, TTime schedulingTime);
        
        public PrinterSchedulingContext<TTime> CreateEmptyContext(
            PrinterExecutionState<TTime> state, TTime schedulingTime);
        
        public PrinterSchedulingContext<TTime> CreatePartiallyFilledContext(
            PrinterExecutionState<TTime> state, 
            TTime schedulingTime, 
            IEnumerable<int> excludedJobsIds);
    }
}