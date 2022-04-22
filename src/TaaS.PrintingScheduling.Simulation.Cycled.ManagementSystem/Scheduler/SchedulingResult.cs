using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public class SchedulingResult<TTime> where TTime : struct
    {
        public SchedulingResult(
            IReadOnlyDictionary<int, IReadOnlySchedulesQueue<TTime>> scheduled,
            IEnumerable<JobSpecification<TTime>>? notScheduled = null)
        {
            Scheduled = scheduled;
            NotScheduled = notScheduled?.ToArray() ?? Array.Empty<JobSpecification<TTime>>();
        }

        public IReadOnlyDictionary<int, IReadOnlySchedulesQueue<TTime>> Scheduled { get; }
        
        public IReadOnlyCollection<JobSpecification<TTime>> NotScheduled { get; }
    }
}