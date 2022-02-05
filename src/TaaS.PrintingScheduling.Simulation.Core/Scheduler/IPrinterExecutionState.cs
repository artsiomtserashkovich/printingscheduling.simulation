using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler
{
    public interface IPrinterExecutionState<TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public TTime CurrentJobFinishTime { get; }
        
        public IReadOnlyCollection<JobSchedule<TTime>> Schedules { get; }
    }
}