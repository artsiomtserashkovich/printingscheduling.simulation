using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem
{
    public class CycledPrintingJobExecutable : IPrintingJobExecutable<long>
    {
        private readonly JobSchedule<long> _schedule;
        private double _remainingVolume;
        private long? _startExecutionTime;

        public CycledPrintingJobExecutable(JobSchedule<long> schedule)
        {
            _schedule = schedule;
            _remainingVolume = Specification.Dimension.Volume;
        }

        public JobSpecification<long> Specification => _schedule.Job;
        
        public PrintingJobIterationResult ExecuteIteration(PrinterSpecification printer, long executionTime)
        {
            _startExecutionTime ??= executionTime;
            
            if (_remainingVolume == 0)
            {
                throw new InvalidOperationException(
                    $"Job can't be executed cause it completed. Job id: '{Specification.Id}'.");
            }
            
            var newRemainingVolume =
                _remainingVolume - CalculateCyclePrintingVolume(printer.Resolution, printer.PrintingSpeed);

            _remainingVolume = newRemainingVolume <= 0 ? 0 : newRemainingVolume;

            return _remainingVolume == 0
                ? new CompletedPrintingIterationResult<long>(
                    _schedule.ScheduleTimeSlot,
                    new TimeSlot<long>(_startExecutionTime.Value, executionTime))
                : new NotCompletePrintingIterationResult(_remainingVolume);
        }

        private double CalculateCyclePrintingVolume(double resolution, double printingSpeed)
        {
            return printingSpeed * (resolution * resolution) * 10;
        }
    }
}