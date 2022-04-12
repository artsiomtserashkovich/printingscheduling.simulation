using System.Collections.Generic;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Queue
{
    public interface ISchedulingQueue<TSchedule, TTime> : IEnumerable<TSchedule> where TTime : struct
    {
        public TTime NextFreeTime { get; }

        public void Enqueue(TSchedule schedule);

        public TSchedule DequeueNextSchedule();
    }
}