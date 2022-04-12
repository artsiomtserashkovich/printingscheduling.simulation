using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Schedules;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingProfile
{
    public interface IPrinterSchedulingContext<TTime> where TTime : struct
    {
        public TTime NextAvailableTime { get; }

        public PrinterSpecification Printer { get; }

        public IReadOnlyCollection<JobSchedule<TTime>> Schedules { get; }
        
        public void ApplySchedule(JobSchedule<TTime> schedule);
    }
}