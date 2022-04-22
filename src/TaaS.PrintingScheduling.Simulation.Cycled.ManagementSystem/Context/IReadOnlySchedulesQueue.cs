using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context
{
    public interface IReadOnlySchedulesQueue<TTime> : IEnumerable<JobSchedule<TTime>> where TTime : struct
    {
        public JobSchedule<TTime>? TryDequeueNextSchedule();
    }
}