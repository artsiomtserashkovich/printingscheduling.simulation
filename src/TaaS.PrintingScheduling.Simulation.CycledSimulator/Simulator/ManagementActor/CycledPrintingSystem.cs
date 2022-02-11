using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.Jobs;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.JobSource;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.WorkloadContext;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor
{
    public class CycledPrintingSystem : ICycledManagementActor, IPrintingSystem
    {
        private readonly CycledSystemWorkloadContext _workloadContext;
        private readonly ICycledPrintingJobsSource _jobsSource;
        private readonly IJobsScheduler<long> _jobsScheduler;
        private readonly IJobResultCollector<long> _resultCollector;

        public CycledPrintingSystem(
            ICycledPrintingJobsSource jobsSource,
            IJobsScheduler<long> jobsScheduler,
            IJobResultCollector<long> resultCollector, 
            CycledSystemWorkloadContext workloadContext)
        {
            _jobsSource = jobsSource;
            _jobsScheduler = jobsScheduler;
            _resultCollector = resultCollector;
            _workloadContext = workloadContext;
        }

        public bool IsComplete => !_jobsSource.IsContainsJobs && _workloadContext.IsComplete;
        
        public void ExecuteManagingCycle(ICycledSimulationContext cycledContext)
        {
            if (_jobsSource.IsContainsJobs)
            {
                var incomingJobs = _jobsSource.GetIncomingJobs(cycledContext);
                if (incomingJobs.Any())
                {
                    var currentPrintersStates = _workloadContext.GetCurrentStates(cycledContext);
                    var schedulingResult = _jobsScheduler.Schedule(incomingJobs, currentPrintersStates);

                    _workloadContext.ApplySchedulingResult(schedulingResult.Scheduled);
                }
            }
        }
        
        public void RegisterFinishedJob(IPrinter printer, JobExecutionResult<long> result)
        {
            _workloadContext.CompleteCurrentJob(printer);
            _resultCollector.RegisterResult(result);
        }

        public ICycledJob? ScheduleNextJob(IPrinter printer)
        {
            return _workloadContext.ScheduledNewJob(printer);
        }
    }
}