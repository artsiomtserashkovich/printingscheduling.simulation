using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Queue;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingProfile;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulingContext
{
    public class CycledSchedulingContextFactory : ISchedulingContextFactory<long>
    {
        public PrinterSchedulingContext<long> CreateFilledContext(PrinterExecutionState<long> state, long schedulingTime)
        {
            if (state.SchedulesQueue is null)
            {
                var nextAvailableTime = (state.CurrentJob?.ScheduleTimeSlot.Finish + 1) ?? schedulingTime;
                
                return new CycledPrinterSchedulingContext(state.Printer, nextAvailableTime, new CycleNoGapOverlapSafeQueue());
            }
            else if (state.SchedulesQueue is CycleNoGapOverlapSafeQueue queue)
            {
                var scheduledJobsFinishTime = (queue.LastEndTime ?? state.CurrentJob?.ScheduleTimeSlot.Finish);
                var nextAvailableTime = (scheduledJobsFinishTime + 1) ?? schedulingTime;
                
                return new CycledPrinterSchedulingContext(
                    state.Printer, 
                    nextAvailableTime, 
                    CycleNoGapOverlapSafeQueue.Clone(queue));
            }
            else
            {
                throw new InvalidOperationException(
                    $"Unsupported type of queue. Type: {state.SchedulesQueue.GetType().Name}");
            }
        }

        public PrinterSchedulingContext<long> CreateEmptySchedulesContext(PrinterExecutionState<long> state)
        {
            throw new System.NotImplementedException();
        }
    }
}