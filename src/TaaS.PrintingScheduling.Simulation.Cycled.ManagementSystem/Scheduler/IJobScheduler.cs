using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public interface IJobScheduler<TTime> where TTime : struct
    {
        public SchedulingResult<TTime> Schedule(
            PrintingJob<TTime> job, 
            IEnumerable<PrinterExecutionState<TTime>> states,
            TTime schedulingTime);
    }
}