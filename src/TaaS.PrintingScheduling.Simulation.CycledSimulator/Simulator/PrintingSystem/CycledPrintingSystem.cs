using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem;
using TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem.Printer;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.JobSource;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrintingSystem
{
    public class CycledPrintingSystem : ICycledManagementActor, IPrintingSystem
    {
        private readonly ICycledPrintingJobsSource _jobsSource;
        private readonly IReadOnlyCollection<PrinterWorkloadContext> _executionContexts;
        private readonly IJobsScheduler<long> _jobsScheduler;

        public CycledPrintingSystem(
            ICycledPrintingJobsSource jobsSource,
            IReadOnlyCollection<PrinterWorkloadContext> executionContexts,
            IJobsScheduler<long> jobsScheduler)
        {
            _jobsSource = jobsSource;
            _executionContexts = executionContexts;
            _jobsScheduler = jobsScheduler;
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
        
        public void RegisterFinishedJob(IPrinter printer, ICycledJob finishedJob, ICycledSimulationContext cycledContext)
        {
            Console.WriteLine($"FinishJob: '{finishedJob.Specification.Id}' at printer: '{printer.Id}'; start time: '{finishedJob.ExecutionStartTime}'; end time: '{finishedJob.ExecutionFinishTime}'");
            _executionContexts
                .First(context => context.Printer.Id == printer.Id)
                .RegisterCompletedJob(finishedJob, cycledContext);
        }

        public ICycledJob? ScheduleNextJob(IPrinter printer, ICycledSimulationContext cycledContext)
        {
            return _executionContexts
                .First(context => context.Printer.Id == printer.Id)
                .StartNextScheduledJob(cycledContext);
        }
    }
}