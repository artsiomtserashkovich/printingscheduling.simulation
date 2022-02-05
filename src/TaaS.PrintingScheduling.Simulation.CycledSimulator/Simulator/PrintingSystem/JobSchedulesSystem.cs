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
        private readonly IReadOnlyCollection<CycledPrinterExecutionContext> _executionContexts;
        private readonly IJobsScheduler<long> _jobsScheduler;

        public CycledPrintingSystem(
            ICycledPrintingJobsSource jobsSource,
            IReadOnlyCollection<CycledPrinterExecutionContext> executionContexts,
            IJobsScheduler<long> jobsScheduler)
        {
            _jobsSource = jobsSource;
            _executionContexts = executionContexts;
            _jobsScheduler = jobsScheduler;
        }

        public bool IsComplete => 
            _jobsSource.IsContainsJobs && _executionContexts.All(context => context.IsFinish);
        
        public void ExecuteManagingCycle(ICycledSimulationContext cycledContext)
        {
            var incomingJobs = _jobsSource.GetIncomingJobs(cycledContext);
            
            var scheduledJobs = _jobsScheduler.Schedule(incomingJobs, _executionContexts);
        }
        
        public void RegisterFinishedJob(ICycledJob finishedJob)
        {
            throw new NotImplementedException();
        }

        public ICycledJob ScheduleNextJob(IPrinter printer)
        {
            throw new NotImplementedException();
        }
    }
}