using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.LeastFinishTime
{
    public struct ScheduleOption<TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public JobSpecification<TTime> Job { get; }
        
        public TimeSlot<TTime> TimeSlot { get; }

        public ScheduleOption(
            PrinterSpecification printer,
            JobSpecification<TTime> job,
            TimeSlot<TTime> timeSlot)
        {
            Printer = printer;
            Job = job;
            TimeSlot = timeSlot;
        }
    }
}