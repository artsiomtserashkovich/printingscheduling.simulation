using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.LeastFinishTime;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Scheduler;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.JobSource;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrintingSystem;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Builders
{
    public class CycledPrintingSystemBuilder
    {
        private readonly IEnumerable<PrinterSpecification> _printers;
        private ICycledPrintingJobsSource _jobsSource;
        private IJobsScheduler<long> _jobsScheduler;

        public CycledPrintingSystemBuilder(IEnumerable<PrinterSpecification> printers)
        {
            _printers = printers;
        }
        
        public CycledPrintingSystemBuilder WithIncomingJobs(
            IReadOnlyCollection<JobSpecification<long>> incomingJobs)
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
            _jobsScheduler = new LeastFinishTimeScheduler<long>(new CycledTimeSlatCalculator());
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
            if (_printers == null)
            {
                throw new InvalidOperationException($"{nameof(_jobsScheduler)} is not initialized.");
            }
            
            return new CycledPrintingSystem(
                _jobsSource, 
                _printers.Select(p => new PrinterWorkloadContext(p)).ToArray(), 
                _jobsScheduler);
        }
    }
}