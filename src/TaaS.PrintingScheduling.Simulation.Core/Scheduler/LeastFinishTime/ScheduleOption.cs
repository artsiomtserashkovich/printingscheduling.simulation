using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.LeastFinishTime
{
    public class ScheduleOption<TTime> where TTime : struct
    {
        public ScheduleOption(IPrinterSchedulingState<TTime> state, ExecutionTimeSlot<TTime> scheduledTimeSlot)
        {
            State = state;
            ScheduledTimeSlot = scheduledTimeSlot;
        }
            
        public IPrinterSchedulingState<TTime> State { get; }
            
        public ExecutionTimeSlot<TTime> ScheduledTimeSlot { get; }

    }
}