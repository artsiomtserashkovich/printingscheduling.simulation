using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions
{
    public class PrioritizedScheduleOption<TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public PrintingJob<TTime> Job { get; }
        
        public TimeSlot<TTime> TimeSlot { get; }
        
        public double TimePriority { get; }
        
        public double ResolutionPriority { get; }
        
        public double TotalPriority => (Job.Specification.PriorityCoefficient * ResolutionPriority) + 
            ((1 - Job.Specification.PriorityCoefficient) * TimePriority);

        public PrioritizedScheduleOption(
            PrinterSpecification printer,
            PrintingJob<TTime> job,
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