using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Schedules;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler
{
    public interface IPrinterSchedulingState<TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public TTime NextSlotStartTime { get; }
        
        public IReadOnlyCollection<JobSchedule<TTime>> Schedules { get; }
    }
}