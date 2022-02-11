using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.Schedules;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.Jobs;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.WorkloadContext
{
    public class CycledSystemWorkloadContext
    {
        private readonly IReadOnlyDictionary<int, CycledPrinterWorkloadContext> _contexts;

        public CycledSystemWorkloadContext(IEnumerable<PrinterSpecification> printers)
        {
            _contexts = printers.ToDictionary(printer => printer.Id, printer => new CycledPrinterWorkloadContext(printer));
        }

        public bool IsComplete => _contexts.Values.All(context => context.IsComplete);
        
        public ICycledJob? ScheduledNewJob(IPrinter printer)
        {
            var nextJobSchedule = GetPrinterContext(printer.Id).StartNextScheduledJob();
            
            return nextJobSchedule != null ? new CycledJob(nextJobSchedule) : null;
        }

        public void CompleteCurrentJob(IPrinter printer)
        {
            GetPrinterContext(printer.Id).CompletedCurrentJob();
        }

        public IReadOnlyCollection<IPrinterSchedulingState<long>> GetCurrentStates(ICycledSimulationContext cycledContext)
        {
            return _contexts.Values.Select(context => context.GetCurrentState(cycledContext)).ToArray();
        }

        public void ApplySchedulingResult(IReadOnlyDictionary<int, IReadOnlyCollection<JobSchedule<long>>> results)
        {
            foreach (var (printerId, schedules) in results)
            {
                GetPrinterContext(printerId).ApplyRescheduling(schedules);
            }
        }

        private CycledPrinterWorkloadContext GetPrinterContext(int printerId)
        {
            return _contexts.TryGetValue(printerId, out var printerContext) 
                ? printerContext 
                : throw new ArgumentException($"Workload context doesn't contains printer with id: '{printerId}'.", nameof(printerId));
        }
    }
}