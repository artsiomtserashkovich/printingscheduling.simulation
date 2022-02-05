using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrintingSystem
{
    public class CycledPrinterExecutionContext : IPrinterExecutionState<long>
    {
        
        public CycledPrinterExecutionContext(PrinterSpecification specification)
        {
            Specification = specification;
            Queue = new Queue<CycledPrintingJobSchedule>();
        }
        
        public PrinterSpecification Specification { get; }
        
        public (int StartExecutingCycle, CycledPrintingJobSchedule Schedule)? CurrentJob { get; private set; }
        
        public Queue<CycledPrintingJobSchedule> Queue { get; }

        public bool IsFinish => CurrentJob == null && !Queue.Any();

        public ICycledJob StartNextScheduledJob(ICycledSimulationContext simulationContext)
        {
            if (CurrentJob != null)
            {
                throw new InvalidOperationException("Previous job didn't completed.");
            }

            var nextJob = Queue.Dequeue();
            if (nextJob != null)
            {
                CurrentJob = (simulationContext.CurrentCycle, nextJob);
            }

            throw new NotImplementedException();
            // return new PrintingJobWithResultReporting(new CycledPrintingJob(nextJob.JobSpecification), this);
        }

        public void RegisterCompletedJob(ICycledJob completedJob, ICycledSimulationContext simulationContext)
        {
            if (CurrentJob?.Schedule.JobSpecification.Id != completedJob.Specification.Id)
            {
                throw new ArgumentException(
                    "Not equal to current scheduled job.", 
                    nameof(completedJob));
            }

            var jobResult = new CycledPrintingJobResult(
                CurrentJob.Value.Schedule,
                Specification,
                CurrentJob.Value.StartExecutingCycle,
                simulationContext.CurrentCycle);
            
            CurrentJob = null;
        }

        public PrinterSpecification Printer { get; }
        public long CurrentJobFinishTime { get; }
        public IReadOnlyCollection<JobSchedule<long>> Schedules { get; }
    }
}