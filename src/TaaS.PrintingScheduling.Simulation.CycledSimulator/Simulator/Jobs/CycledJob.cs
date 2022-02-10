using System;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.Jobs
{
    public class CycledJob : ICycledJob
    {
        private readonly JobSchedule<long> _jobSchedule;
        private double _remainingVolume;
        private long? _executionStartTime;
        private long? _executionFinishTime;

        public CycledJob(JobSchedule<long> schedule)
        {
            _jobSchedule = schedule;
            _remainingVolume = Specification.Dimension.Volume;
        }

        public bool IsComplete => _remainingVolume == 0;

        public JobSpecification<long> Specification => _jobSchedule.Job;
        
        public JobExecutionResult<long> GetResultReport(PrinterSpecification printer)
        {
            if (!IsComplete)
            {
                throw new InvalidOperationException(
                    $"Not able to generate result report for job with id: '{printer.Id}'. Job isn't completed.");
            }
            if (_executionStartTime == null || _executionFinishTime == null)
            {
                throw new InvalidOperationException(
                    $"Not able to generate result: {nameof(_executionStartTime)} or {nameof(_executionFinishTime)} not defined.");
            }

            var scheduledTimeSlot = _jobSchedule.TimeSlot;
            var executionTimeSlot = new TimeSlot<long>(_executionStartTime.Value, _executionFinishTime.Value);

            return new JobExecutionResult<long>(
                Specification.Id, 
                printer.Id, 
                Specification.IncomingTime, 
                scheduledTimeSlot, 
                executionTimeSlot);
        }

        public void Execute(PrinterSpecification printer, ICycledSimulationContext cycledContext)
        {
            if (IsComplete)
            {
                throw new InvalidOperationException(
                    $"Job can't be executed cause it completed. Job id: '{Specification.Id}'.");
            }
            if (_executionStartTime is null)
            {
                _executionStartTime = cycledContext.CurrentCycle;
            }
            
            ExecExecuteImplementation(printer, cycledContext);
        }

        private void ExecExecuteImplementation(PrinterSpecification printer, ICycledSimulationContext cycledContext)
        {
            var resolutionFactor = printer.Resolution * printer.Resolution * 10;
            var cyclePrintingVolume =  resolutionFactor * printer.PrintingSpeed;
            var newRemainingVolume =_remainingVolume - cyclePrintingVolume;
            
            if (newRemainingVolume <= 0)
            {
                _executionFinishTime = cycledContext.CurrentCycle;
                newRemainingVolume = 0;
            }
            
            _remainingVolume = newRemainingVolume;
        }
    }
}