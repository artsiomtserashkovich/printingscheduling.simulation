using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.LeastFinishTime;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.FixBoundTime;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Scheduler;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Scheduler.PriorityCalculation.TimePriorityCalculator;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Scheduler.SchedulingProfile;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.JobSource;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.WorkloadContext;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Builders
{
    public class CycledPrintingSystemBuilder
    {
        private readonly IEnumerable<PrinterSpecification> _printers;
        private readonly IJobResultCollector<long> _resultCollector;

        private ICycledPrintingJobsSource _jobsSource;
        private IJobsScheduler<long> _jobsScheduler;

        public CycledPrintingSystemBuilder(
            IEnumerable<PrinterSpecification> printers, 
            IJobResultCollector<long> resultCollector)
        {
            _printers = printers;
            _resultCollector = resultCollector;
        }
        
        public CycledPrintingSystemBuilder WithIncomingJobs(IEnumerable<JobSpecification<long>> incomingJobs)
        {
            var priorityQueue = new PriorityQueue<JobSpecification<long>, long>();
            foreach (var job in incomingJobs)
            {
                priorityQueue.Enqueue(job, job.IncomingTime);
            }
            
            _jobsSource = new CycledIncomingJobsSource(priorityQueue);
            return this;
        }
        
        public CycledPrintingSystemBuilder WithLeastFinishTimeScheduler()
        {
            _jobsScheduler = new LeastFinishTimeScheduler<long>(new CycledTimeSlotCalculator(), new CycledPrinterSchedulingContextFactory());
            
            return this;
        }
        
        public CycledPrintingSystemBuilder WithFixBoundTimeScheduler(double resolutionThreshold = 0.2)
        {
            _jobsScheduler = new FixBoundTimeScheduler<long>(
                new CycledTimeSlotCalculator(), 
                new LinearTimePriorityCalculator(),
                new LinearResolutionPriorityCalculator(resolutionThreshold),
                new CycledPrinterSchedulingContextFactory());
            
            return this;
        }
        
        public CycledPrintingSystem Build()
        {
            if (_jobsSource == null)
            {
                throw new InvalidOperationException($"{nameof(_jobsSource)} is not initialized.");
            }
            if (_jobsScheduler == null)
            {
                throw new InvalidOperationException($"{nameof(_jobsScheduler)} is not initialized.");
            }
            if (_printers == null || !_printers.Any())
            {
                throw new InvalidOperationException($"{nameof(_printers)} is not initialized.");
            }
            
            return new CycledPrintingSystem(
                _jobsSource,
                _jobsScheduler,
                _resultCollector,
                new CycledSystemWorkloadContext(_printers));
        }
    }
}