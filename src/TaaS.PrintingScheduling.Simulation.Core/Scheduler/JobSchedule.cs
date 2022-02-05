using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler
{
    public class JobSchedule<TTime> where TTime : struct
    {
        public JobSpecification<TTime> Job { get; }

        public TTime ScheduledStartTime { get; }
        
        public TTime ExpectedFinishTime { get; }

        public JobSchedule(JobSpecification<TTime> job, TTime start, TTime finish)
        {
            Job = job;
            ScheduledStartTime = start;
            ExpectedFinishTime = finish;
        }
    }
}