using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public class SchedulingResult<TTime> where TTime : struct
    {
        private SchedulingResult(
            bool isScheduled, 
            IReadOnlyDictionary<int, IReadOnlySchedulesQueue<TTime>> changedStates)
        {
            IsScheduled = isScheduled;
            ChangedStates = changedStates;
        }
        
        public bool IsScheduled { get; }

        public IReadOnlyDictionary<int, IReadOnlySchedulesQueue<TTime>> ChangedStates { get; }

        public static SchedulingResult<TTime> NotScheduled()
        {
            return new SchedulingResult<TTime>(false, new Dictionary<int, IReadOnlySchedulesQueue<TTime>>());
        }

        public static SchedulingResult<TTime> Scheduled(
            IReadOnlyDictionary<int, IReadOnlySchedulesQueue<TTime>> changedStates)
        {
            return new SchedulingResult<TTime>(true, changedStates);
        }
    }
}