using TaaS.PrintingScheduling.Simulation.Cycled.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.IncomingJobsQueue;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;
using TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem
{
    public class CycledPrintingSystem : ICycledManagementActor, IPrintingSystem<long>
    {
        private readonly CycledSystemWorkloadContext _workloadContext;
        private readonly IIncomingJobsQueue _jobsQueue;
        private readonly IJobScheduler<long> _jobScheduler;

        public CycledPrintingSystem(
            IIncomingJobsQueue jobsQueue,
            IJobScheduler<long> jobScheduler,
            CycledSystemWorkloadContext workloadContext)
        {
            _jobsQueue = jobsQueue;
            _jobScheduler = jobScheduler;
            _workloadContext = workloadContext;
        }

        public bool IsComplete => !_jobsQueue.IsContainsJobs && _workloadContext.IsComplete;
        
        public void ExecuteCycle(ICycledSimulationContext cycledContext)
        {
            var incomingJob = _jobsQueue.Dequeue(cycledContext);
            if (incomingJob != null)
            {
                var currentPrintersStates = _workloadContext.GetCurrentStates();
                var schedulingResult = _jobScheduler.Schedule(incomingJob, currentPrintersStates, cycledContext.CurrentCycle);

                if (!schedulingResult.IsScheduled)
                {
                    throw new InvalidOperationException($"Job was't scheduled. Job id: '{incomingJob.Specification.Id}'.");
                }

                _workloadContext.ApplySchedulingResult(schedulingResult.ChangedStates);
            }
        }
        
        public void RegisterFinishedJob(IPrinter printer)
        {
            _workloadContext.CompleteCurrentJob(printer.Id);
        }

        public IPrintingJobExecutable<long>? ScheduleNextJob(IPrinter printer)
        {
            return _workloadContext.ScheduledNewJob(printer.Id);
        }
    }
}