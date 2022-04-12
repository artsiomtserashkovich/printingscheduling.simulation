using System.Collections.Generic;
using System.Linq;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingProfile
{
    public class CycledPrinterSchedulingContextFactory : IPrinterSchedulingContextFactory<long>
    {
        public IReadOnlyCollection<IPrinterSchedulingContext<long>> CreateFilledContexts(IEnumerable<IPrinterSchedulingState<long>> states)
        {
            return states.Select(CycledPrinterSchedulingContext.FromState).ToArray();
        }
    }
}