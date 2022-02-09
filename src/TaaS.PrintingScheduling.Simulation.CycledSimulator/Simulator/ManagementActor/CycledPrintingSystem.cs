using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.Jobs;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.JobSource;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor
{
    public class CycledPrintingSystem : ICycledManagementActor, IPrintingSystem
    {
        private readonly ICycledPrintingJobsSource _jobsSource;
        private readonly IReadOnlyCollection<PrinterWorkloadContext> _executionContexts;
        private readonly IJobsScheduler<long> _jobsScheduler;
        private readonly IJobResultCollector<long> _resultCollector;

        public CycledPrintingSystem(
            ICycledPrintingJobsSource jobsSource,
            IReadOnlyCollection<PrinterWorkloadContext> executionContexts,
            IJobsScheduler<long> jobsScheduler,
            IJobResultCollector<long> resultCollector)
        {
            _jobsSource = jobsSource;
            _executionContexts = executionContexts;
            _jobsScheduler = jobsScheduler;
            _resultCollector = resultCollector;
        }

        public bool IsComplete => !_jobsSource.IsContainsJobs && _executionContexts.All(context => context.IsEmpty);
        
        public void ExecuteManagingCycle(ICycledSimulationContext cycledContext)
        {
            if (_jobsSource.IsContainsJobs)
            {
                var incomingJobs = _jobsSource.GetIncomingJobs(cycledContext);
                _jobsScheduler.Schedule(incomingJobs, _executionContexts, cycledContext.CurrentCycle);
            }
        }
        
        public void RegisterFinishedJob(JobExecutionResult<long> result)
        {
            _executionContexts
                .First(context => context.Printer.Id == result.PrinterId)
                .CompletedCurrentJob();
            
            _resultCollector.RegisterResult(result);
        }

        public ICycledJob? ScheduleNextJob(IPrinter printer)
        {
            return _executionContexts
                .First(context => context.Printer.Id == printer.Id)
                .StartNextScheduledJob();
        }
    }
}