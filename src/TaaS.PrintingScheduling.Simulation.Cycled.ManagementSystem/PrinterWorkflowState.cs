using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem
{
    public class PrinterWorkflowState : IPrinterSchedulingState<long>
    {
        public PrinterWorkflowState(
            IReadOnlyCollection<JobSchedule<long>> schedules, 
            long activityExpectedFinishTime, 
            PrinterSpecification printer)
        {
            Schedules = schedules;
            NextSlotStartTime = activityExpectedFinishTime;
            Printer = printer;
        }
            
        public PrinterSpecification Printer { get; }
            
        public long NextSlotStartTime { get; }
            
        public IReadOnlyCollection<JobSchedule<long>> Schedules { get; }
    }
}