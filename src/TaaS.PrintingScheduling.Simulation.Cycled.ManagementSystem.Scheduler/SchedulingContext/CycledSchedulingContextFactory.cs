using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext.Queue;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext
{
    public class CycledSchedulingContextFactory : ISchedulingContextFactory<long>
    {
        public PrinterSchedulingContext<long> CreateFilledContext(
            PrinterExecutionState<long> state, long schedulingTime)
        {
            if (state.SchedulesQueue is null)
            {
                var nextAvailableTime = (state.CurrentJob?.TimeSlot.Finish + 1) ?? schedulingTime;
                
                return new CycledPrinterSchedulingContext(state.Printer, nextAvailableTime, new CycleDistinctNoGapOverlapSafeQueue());
            }
            else if (state.SchedulesQueue is CycleDistinctNoGapOverlapSafeQueue queue)
            {
                var scheduledJobsFinishTime = (queue.LastEndTime ?? state.CurrentJob?.TimeSlot.Finish);
                var nextAvailableTime = (scheduledJobsFinishTime + 1) ?? schedulingTime;
                
                return new CycledPrinterSchedulingContext(
                    state.Printer, 
                    nextAvailableTime, 
                    CycleDistinctNoGapOverlapSafeQueue.Clone(queue));
            }
            else
            {
                throw new InvalidOperationException(
                    $"Unsupported type of queue. Type: {state.SchedulesQueue.GetType().Name}");
            }
        }

        public PrinterSchedulingContext<long> CreateEmptyContext(
            PrinterExecutionState<long> state, long schedulingTime)
        {
            var nextAvailableTime = (state.CurrentJob?.TimeSlot.Finish + 1) ?? schedulingTime;
            
            return new CycledPrinterSchedulingContext(
                state.Printer, 
                nextAvailableTime,
                new CycleDistinctNoGapOverlapSafeQueue());
        }
    }
}