using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingProfile;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Scheduler.SchedulingProfile
{
    public class CycledPrinterSchedulingContextFactory : IPrinterSchedulingContextFactory<long>
    {
        public IReadOnlyCollection<IPrinterSchedulingContext<long>> CreateFilledContexts(IEnumerable<IPrinterSchedulingState<long>> states)
        {
            return states.Select(CycledPrinterSchedulingContext.FromState).ToArray();
        }
    }
}