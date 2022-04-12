using System;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Cycled.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.IncomingJobsQueue;
using TaaS.PrintingScheduling.Simulation.Cycled.Jobs;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;
using TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem
{
    public class CycledPrintingSystem : ICycledManagementActor, IPrintingSystem
    {
        private readonly CycledSystemWorkloadContext _workloadContext;
        private readonly IIncomingJobsQueue _jobsQueue;
        private readonly IJobsScheduler<long> _jobsScheduler;

        public CycledPrintingSystem(
            IIncomingJobsQueue jobsQueue,
            IJobsScheduler<long> jobsScheduler,
            CycledSystemWorkloadContext workloadContext)
        {
            _jobsQueue = jobsQueue;
            _jobsScheduler = jobsScheduler;
            _workloadContext = workloadContext;
        }

        public bool IsComplete => !_jobsQueue.IsContainsJobs && _workloadContext.IsComplete;
        
        public void ExecuteCycle(ICycledSimulationContext cycledContext)
        {
            var incomingJobs = _jobsQueue.Dequeue(cycledContext);
            if (incomingJobs.Any())
            {
                var currentPrintersStates = _workloadContext.GetCurrentStates(cycledContext);
                var schedulingResult = _jobsScheduler.Schedule(incomingJobs, currentPrintersStates);

                if (schedulingResult.NotScheduled.Any())
                {
                    var stringIds = string.Join(',', schedulingResult.NotScheduled.Select(j => j.Id));
                    
                    throw new InvalidOperationException($"Jobs weren't scheduled. Jobs ids: '{stringIds}'.");
                }

                _workloadContext.ApplySchedulingResult(schedulingResult.Scheduled);
            }
        }
        
        public void RegisterFinishedJob(IPrinter printer)
        {
            _workloadContext.CompleteCurrentJob(printer);
        }

        public ICycledJob? ScheduleNextJob(IPrinter printer)
        {
            return _workloadContext.ScheduledNewJob(printer);
        }
    }
}