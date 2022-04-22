using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;

namespace TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor
{
    public class CycledPrinterActor : IPrinter, ICycledPrinterActor
    {
        private readonly IPrintingSystem<long> _printingSystem;
        private readonly PrinterSpecification _specification;

        private IPrintingJobExecutable<long>? _currentJob = null;
        
        public CycledPrinterActor(
            PrinterSpecification specification,
            IPrintingSystem<long> printingSystem)
        {
            _specification = specification;
            _printingSystem = printingSystem;
        }

        public int Id => _specification.Id;
        
        private bool IsFree => _currentJob == null;

        public void ExecuteCycle(ICycledSimulationContext simulationContext)
        {
            if (IsFree)
            {
                var newIncomingJob = _printingSystem.ScheduleNextJob(this);
            
                AssertIncomingJob(newIncomingJob);
                _currentJob = newIncomingJob;
            }

            var result = _currentJob?.ExecuteIteration(_specification, simulationContext.CurrentCycle);
            
            if (result?.IsComplete ?? false)
            {
                if (result is CompletedPrintingIterationResult<long> completedResult)
                {
                    var executionResult = new JobExecutionResult<long>(
                        _currentJob.Specification,
                        _specification,
                        completedResult.ScheduledTime,
                        completedResult.ExecutionTime);
                    
                    simulationContext.RegisterResult(executionResult);
                    _printingSystem.RegisterFinishedJob(this);
                    
                    _currentJob = null;
                }
                else
                {
                    throw new InvalidOperationException("Not supported type of the result.");
                }
            }
        }

        private void AssertIncomingJob(IPrintingJobExecutable<long>? job)
        {
            if (job != null && job.Specification.Dimension > _specification.PrintingDimension)
            {
                _currentJob = null;
                throw new InvalidOperationException(
                    $"Dimension of the job with id='{job.Specification.Id}' is more then printer dimension.");
            }
        }
    }
}