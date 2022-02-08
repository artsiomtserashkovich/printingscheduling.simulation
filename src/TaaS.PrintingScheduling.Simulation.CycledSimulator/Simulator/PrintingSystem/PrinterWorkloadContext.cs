using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;
using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor.Jobs;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrintingSystem
{
    public class PrinterWorkloadContext : IPrinterSchedulingState<long>
    {
        private readonly Queue<JobSchedule<long>> _queue;
        
        public PrinterWorkloadContext(PrinterSpecification printer)
        {
            Printer = printer;
            _queue = new Queue<JobSchedule<long>>();
        }

        public Queue<JobSchedule<long>> Schedules => _queue;
        
        public PrinterSpecification Printer { get; }
        
        public JobSchedule<long>? CurrentJob { get; private set; }

        public bool IsEmpty => CurrentJob == null && !_queue.Any();

        public ICycledJob? StartNextScheduledJob(ICycledSimulationContext simulationContext)
        {
            if (CurrentJob != null)
            {
                throw new InvalidOperationException("Previous job didn't completed.");
            }

            if (_queue.Any())
            {
                var nextJob = _queue.Dequeue();
                CurrentJob = nextJob;
                
                return new CycledJob(nextJob.Job);
            }

            return null;
        }

        public void RegisterCompletedJob(ICycledJob completedJob, ICycledSimulationContext simulationContext)
        {
            if (CurrentJob?.Job.Id != completedJob.Specification.Id)
            {
                throw new ArgumentException(
                    "Not equal to current scheduled job.", 
                    nameof(completedJob));
            }

            CurrentJob = null;
        }
    }
}