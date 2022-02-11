using System.Collections.Generic;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingProfile
{
    public interface IPrinterSchedulingContextFactory<TTime> where TTime : struct
    {
        public IReadOnlyCollection<IPrinterSchedulingContext<TTime>> CreateFilledContexts(
            IEnumerable<IPrinterSchedulingState<TTime>> states);
    }
}