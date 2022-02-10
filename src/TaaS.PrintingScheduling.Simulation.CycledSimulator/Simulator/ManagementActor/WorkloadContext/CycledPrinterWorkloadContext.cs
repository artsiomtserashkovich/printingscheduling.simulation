using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.Jobs;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.WorkloadContext
{
    public class CycledPrinterWorkloadContext
    {
        private readonly PrinterSpecification _printer;
        private readonly Queue<JobSchedule<long>> _queue;
        
        private JobSchedule<long>? _currentJob;
        
        public CycledPrinterWorkloadContext(PrinterSpecification printer)
        {
            _printer = printer;
            _queue = new Queue<JobSchedule<long>>();
        }

        public bool IsComplete => _currentJob == null && !_queue.Any();

        public JobSchedule<long>? StartNextScheduledJob()
        {
            if (_currentJob != null)
            {
                throw new InvalidOperationException("Previous job didn't completed.");
            }
            
            return _queue.Any() ? _queue.Dequeue() : null;
        }

        public void CompletedCurrentJob()
        {
            _currentJob = null;
        }

        public IPrinterSchedulingState<long> GetCurrentState(ICycledSimulationContext context)
        {
            var nextSlotStartTime = _currentJob?.TimeSlot.Finish + 1 ?? context.CurrentCycle;
            
            return new PrinterWorkflowState(_queue.ToArray(), nextSlotStartTime, _printer);
        }

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
}