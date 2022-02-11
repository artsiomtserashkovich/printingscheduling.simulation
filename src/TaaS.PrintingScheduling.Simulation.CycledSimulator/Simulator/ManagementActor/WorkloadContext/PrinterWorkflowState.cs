using System.Collections.Generic;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.Schedules;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.WorkloadContext
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