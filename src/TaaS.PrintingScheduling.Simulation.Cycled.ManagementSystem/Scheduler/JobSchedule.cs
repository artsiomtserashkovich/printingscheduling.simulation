using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public class JobSchedule<TTime> where TTime : struct
    {
        public PrintingJob<TTime> Job { get; }

        public TimeSlot<TTime> TimeSlot { get; }

        public JobSchedule(PrintingJob<TTime> job, TimeSlot<TTime> timeSlot)
        {
            Job = job;
            TimeSlot = timeSlot;
        }
    }
}