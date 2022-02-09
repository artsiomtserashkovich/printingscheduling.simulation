using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.FixBoundTime
{
    public class PrioritizedScheduleOption<TTime> where TTime : struct
    {
        public PrioritizedScheduleOption(
            IPrinterSchedulingState<TTime> state, 
            TimeSlot<TTime> scheduledTimeSlot,
            double resolutionPriority)
        {
            State = state;
            ScheduledTimeSlot = scheduledTimeSlot;
            ResolutionPriority = resolutionPriority;
        }
            
        public IPrinterSchedulingState<TTime> State { get; }
            
        public TimeSlot<TTime> ScheduledTimeSlot { get; }
        
        public double ResolutionPriority { get; }
        
        public double? TimePriority { get; set; }
    }
}