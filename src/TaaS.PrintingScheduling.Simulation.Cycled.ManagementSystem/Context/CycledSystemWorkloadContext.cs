using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context
{
    public class CycledSystemWorkloadContext
    {
        private readonly IReadOnlyDictionary<int, ManagementPrinterContext> _contexts;

        public CycledSystemWorkloadContext(IEnumerable<PrinterSpecification> printers)
        {
            _contexts = printers.ToDictionary(printer => printer.Id, printer => new ManagementPrinterContext(printer));
        }

        public bool IsComplete => _contexts.Values.All(context => context.IsComplete);
        
        public IPrintingJobExecutable<long>? ScheduledNewJob(int printerId)
        {
            var nextJobSchedule = GetPrinterContext(printerId).StartNextScheduledJob();
            
            return nextJobSchedule != null ? new CycledPrintingJobExecutable(nextJobSchedule) : null;
        }

        public void CompleteCurrentJob(int printerId)
        {
            GetPrinterContext(printerId).CompletedCurrentJob();
        }

        public IReadOnlyCollection<PrinterExecutionState<long>> GetCurrentStates()
        {
            return _contexts.Values.Select(context => context.State).ToArray();
        }

        public void ApplySchedulingResult(IReadOnlyDictionary<int, IReadOnlySchedulesQueue<long>> results)
        {
            foreach (var (printerId, schedules) in results)
            {
                GetPrinterContext(printerId).ApplyScheduling(schedules);
            }
        }

        private ManagementPrinterContext GetPrinterContext(int printerId)
        {
            return _contexts.TryGetValue(printerId, out var printerContext) 
                ? printerContext 
                : throw new ArgumentException($"Workload context doesn't contains printer with id: '{printerId}'.", nameof(printerId));
        }
    }
}