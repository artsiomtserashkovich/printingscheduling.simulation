using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions
{
    public class ScheduleOption<TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public PrintingJob<TTime> Job { get; }
        
        public TimeSlot<TTime> TimeSlot { get; }

        public ScheduleOption(
            PrinterSpecification printer,
            PrintingJob<TTime> job,
            TimeSlot<TTime> timeSlot)
        {
            Printer = printer;
            Job = job;
            TimeSlot = timeSlot;
        }
    }
}