using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.TimeAndResolutionPrioritized
{
    public struct PrioritizedScheduleOption<TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public JobSpecification<TTime> Job { get; }
        
        public TimeSlot<TTime> TimeSlot { get; }
        
        public double TimePriority { get; }
        
        public double ResolutionPriority { get; }

        public PrioritizedScheduleOption(
            PrinterSpecification printer,
            JobSpecification<TTime> job,
            TimeSlot<TTime> timeSlot,
            double timePriority,
            double resolutionPriority)
        {
            Printer = printer;
            Job = job;
            TimeSlot = timeSlot;
            TimePriority = timePriority;
            ResolutionPriority = resolutionPriority;
        }
    }
}