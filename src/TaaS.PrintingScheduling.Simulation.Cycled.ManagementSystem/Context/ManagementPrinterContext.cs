using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context
{
    public class ManagementPrinterContext
    {
        private readonly PrinterSpecification _printer;
        
        private JobSchedule<long>? _currentJob;
        private IReadOnlySchedulesQueue<long>? _schedulesQueue;
        
        public ManagementPrinterContext(PrinterSpecification printer)
        {
            _printer = printer;
        }

        public PrinterExecutionState<long> State => new (_printer, _currentJob, _schedulesQueue);
        
        public bool IsComplete => _currentJob == null && !(_schedulesQueue?.Any() ?? false);

        public JobSchedule<long>? StartNextScheduledJob()
        {
            if (_currentJob != null)
            {
                throw new InvalidOperationException("Previous job didn't completed.");
            }
            
            _currentJob = _schedulesQueue?.TryDequeueNextSchedule();

            return _currentJob;
        }

        public void CompletedCurrentJob()
        {
            _currentJob = null;
        }

        public void ApplyScheduling(IReadOnlySchedulesQueue<long> workloadQueue)
        {
            _schedulesQueue = workloadQueue;
        }
    }
}