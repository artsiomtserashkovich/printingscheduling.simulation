using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext.Queue
{
    public interface ISchedulesQueue<TTime> : IReadOnlySchedulesQueue<TTime> where TTime : struct
    {
        public int Count { get; }

        public void Enqueue(JobSchedule<TTime> schedule);
    }
}