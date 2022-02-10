using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.ScheduleOptions
{
    public class ScheduleOption<TTime> where TTime : struct
    {
        public ScheduleOption(IPrinterSchedulingState<TTime> state, TimeSlot<TTime> scheduledTimeSlot)
        {
            State = state;
            ScheduledTimeSlot = scheduledTimeSlot;
        }
            
        public IPrinterSchedulingState<TTime> State { get; }
            
        public TimeSlot<TTime> ScheduledTimeSlot { get; }

    }
}