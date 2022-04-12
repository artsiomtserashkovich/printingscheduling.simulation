using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.SchedulingPolicy;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingProfile
{
    public class SchedulingContext<TTime> where TTime : struct
    {
        private readonly IEnumerable<IPrinterSchedulingContext<TTime>> _printersProfile;
        private readonly ISchedulingPolicy<TTime> _policy;

        public SchedulingContext(
            IEnumerable<IPrinterSchedulingContext<TTime>> printersProfile,
            ISchedulingPolicy<TTime> policy)
        {
            _printersProfile = printersProfile;
            _policy = policy;
        }
        
        
    }
}