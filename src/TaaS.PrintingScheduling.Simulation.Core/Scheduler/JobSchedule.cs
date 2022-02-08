using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler
{
    public class JobSchedule<TTime> where TTime : struct
    {
        public JobSpecification<TTime> Job { get; }

        public ExecutionTimeSlot<TTime> TimeSlot { get; }

        public JobSchedule(JobSpecification<TTime> job, ExecutionTimeSlot<TTime> timeSlot)
        {
            Job = job;
            TimeSlot = timeSlot;
        }
    }
}