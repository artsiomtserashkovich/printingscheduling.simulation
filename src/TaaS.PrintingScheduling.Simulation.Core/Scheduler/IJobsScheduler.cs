using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler
{
    public interface IJobsScheduler<TTime> 
        where TTime : struct
    {
        public IReadOnlyCollection<(PrinterSpecification Printer, IReadOnlyCollection<JobSchedule<TTime>> Schedules)> Schedule(
            IReadOnlyCollection<JobSpecification<TTime>> incomingJobs, 
            IReadOnlyCollection<IPrinterExecutionState<TTime>> printerExecutionStates);
    }
}