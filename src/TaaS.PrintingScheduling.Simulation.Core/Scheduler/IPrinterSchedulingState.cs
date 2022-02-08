using System.Collections.Generic;
using System.Collections.Immutable;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler
{
    public interface IPrinterSchedulingState<TTime> where TTime : struct
    {
        public PrinterSpecification Printer { get; }
        
        public Queue<JobSchedule<TTime>> Schedules { get; }
    }
}