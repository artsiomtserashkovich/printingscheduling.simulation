using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context
{
    public class PrinterExecutionState<TTime> where TTime : struct
    {
        public PrinterExecutionState(
            PrinterSpecification printer,
            JobSchedule<TTime>? currentJob,
            IReadOnlySchedulesQueue<TTime>? schedulesQueue)
        {
            Printer = printer;
            CurrentJob = currentJob;
            SchedulesQueue = schedulesQueue;
        }
            
        public PrinterSpecification Printer { get; }
        
        public JobSchedule<TTime>? CurrentJob { get; }
        
        public IReadOnlySchedulesQueue<TTime>? SchedulesQueue { get; }
    }
}