using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.ScheduleOptions
{
    public class PrioritizedScheduleOption<TTime> where TTime : struct
    {
        public PrioritizedScheduleOption(
            ScheduleOption<TTime> option,
            double resolutionPriority,
            double timePriority) 
            : this(option.State, option.ScheduledTimeSlot, resolutionPriority, timePriority)
        { }
        
        public PrioritizedScheduleOption(
            IPrinterSchedulingState<TTime> state, 
            TimeSlot<TTime> scheduledTimeSlot,
            double resolutionPriority,
            double timePriority)
        {
            State = state;
            ScheduledTimeSlot = scheduledTimeSlot;
            ResolutionPriority = resolutionPriority;
            TimePriority = timePriority;
        }
            
        public IPrinterSchedulingState<TTime> State { get; }
            
        public TimeSlot<TTime> ScheduledTimeSlot { get; }
        
        public double ResolutionPriority { get; }
        
        public double TimePriority { get; }
    }
}