using System.Collections;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingProfile.Queue;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Queue
{
    public class CycleNoGapOverlapSafeQueue : ISchedulesQueue<long>
    {
        private readonly Queue<JobSchedule<long>> _queue;

        public int Count => _queue.Count;

        public long? LastEndTime => _queue.LastOrDefault()?.ScheduleTimeSlot.Finish;

        public CycleNoGapOverlapSafeQueue() : this(new Queue<JobSchedule<long>>())
        {
        }

        private CycleNoGapOverlapSafeQueue(Queue<JobSchedule<long>> initialQueue)
        {
            _queue = new Queue<JobSchedule<long>>();

            foreach (var schedule in initialQueue)
            {
                Enqueue(schedule);
            }
        }

        public void Enqueue(JobSchedule<long> schedule)
        {
            var previous = _queue.LastOrDefault();
            if (previous != null)
            {
                if ((previous.ScheduleTimeSlot.Finish + 1) != schedule.ScheduleTimeSlot.Start)
                {
                    throw new InvalidOperationException("There is gap or overlap between last previous schedule and new schedule.");
                }
            }

            _queue.Enqueue(schedule);
        }

        public JobSchedule<long>? TryDequeueNextSchedule()
        {
            return _queue.TryDequeue(out var schedule) ? schedule : null;
        }

        public IEnumerator<JobSchedule<long>> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static CycleNoGapOverlapSafeQueue Clone(CycleNoGapOverlapSafeQueue queue)
        {
            return new(queue._queue);
        }
    }
}