using System.Collections;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext.Queue
{
    public class CycleDistinctNoGapOverlapSafeQueue : ISchedulesQueue<long>
    {
        private readonly Queue<JobSchedule<long>> _queue;

        public int Count => _queue.Count;

        public long? LastEndTime => _queue.LastOrDefault()?.TimeSlot.Finish;

        public CycleDistinctNoGapOverlapSafeQueue() : this(new Queue<JobSchedule<long>>())
        {
        }

        private CycleDistinctNoGapOverlapSafeQueue(Queue<JobSchedule<long>> initialQueue)
        {
            _queue = new Queue<JobSchedule<long>>();

            foreach (var schedule in initialQueue)
            {
                Enqueue(schedule);
            }
        }

        public void Enqueue(JobSchedule<long> schedule)
        {
            if (_queue.Any(s => s.Job.Specification.Id == schedule.Job.Specification.Id))
            {
                throw new InvalidOperationException(
                    $"Job has already scheduled. Job id: {schedule.Job.Specification.Id}.");
            }
            
            var previous = _queue.LastOrDefault();
            if (previous != null)
            {
                if ((previous.TimeSlot.Finish + 1) != schedule.TimeSlot.Start)
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

        public static CycleDistinctNoGapOverlapSafeQueue Clone(CycleDistinctNoGapOverlapSafeQueue queue)
        {
            return new(queue._queue);
        }
    }
}