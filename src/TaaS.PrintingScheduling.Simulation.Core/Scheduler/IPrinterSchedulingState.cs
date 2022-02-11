using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.Schedules;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler
{
    public interface IPrinterSchedulingState<TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public TTime NextSlotStartTime { get; }
        
        public IReadOnlyCollection<JobSchedule<TTime>> Schedules { get; }
    }
}