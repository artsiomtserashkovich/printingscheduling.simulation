using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public class JobSchedule<TTime> where TTime : struct
    {
        public JobSpecification<TTime> Job { get; }

        public TimeSlot<TTime> ScheduleTimeSlot { get; }

        public JobSchedule(JobSpecification<TTime> job, TimeSlot<TTime> scheduleTimeSlot)
        {
            Job = job;
            ScheduleTimeSlot = scheduleTimeSlot;
        }
    }
}