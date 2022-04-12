using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public class SchedulingResult<TTime> where TTime : struct
    {
        public SchedulingResult(
            IReadOnlyDictionary<int, IReadOnlyCollection<JobSchedule<TTime>>> scheduled,
            IEnumerable<JobSpecification<TTime>> notScheduled = null)
        {
            Scheduled = scheduled;
            NotScheduled = notScheduled?.ToArray() ?? Array.Empty<JobSpecification<TTime>>();
        }

        public IReadOnlyDictionary<int, IReadOnlyCollection<JobSchedule<TTime>>> Scheduled { get; }
        
        public IReadOnlyCollection<JobSpecification<TTime>> NotScheduled { get; }
    }
}