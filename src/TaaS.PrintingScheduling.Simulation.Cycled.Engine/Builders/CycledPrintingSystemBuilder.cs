using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.IncomingJobsQueue;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.LeastFinishTime;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulesQuery;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.TimeAndResolutionPrioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Time;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PriorityCalculation.Time;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingPolicy;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ProfilesTree;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ScheduleOption;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.TimeSlotCalculator;

namespace TaaS.PrintingScheduling.Simulation.Cycled.Engine.Builders
{
    public class CycledPrintingSystemBuilder
    {
        private readonly IEnumerable<PrinterSpecification> _printers;

        private IIncomingJobsQueue _jobsQueue;
        private IJobScheduler<long> _jobScheduler;

        public CycledPrintingSystemBuilder(IEnumerable<PrinterSpecification> printers)
        {
            _printers = printers;
        }

        public CycledPrintingSystemBuilder WithIncomingJobs(IEnumerable<JobSpecification<long>> incomingJobs)
        {
            var priorityQueue = new PriorityQueue<JobSpecification<long>, long>();
            foreach (var job in incomingJobs)
            {
                priorityQueue.Enqueue(job, job.IncomingTime);
            }

            _jobsQueue = new CycledIncomingJobsQueue(priorityQueue);
            return this;
        }

        public CycledPrintingSystemBuilder WithLeastFinishTimeScheduler()
        {
            _jobScheduler = new UnprioritizedJobScheduler<long>(
                new PolicyScheduleOptionsQuery<long>(
                    new CycledTimeSlotCalculator(), 
                    new DeadlineTimePolicy(new NotFitDimensionsPolicy<long>(new WorseResolutionPolicy<long>()))),
                new LeastFinishTimeOptionChooser<long>(),
                new CycledSchedulingContextFactory());

            return this;
        }

        public CycledPrintingSystemBuilder WithFixBoundTimeScheduler(double resolutionThreshold = 0.2)
        {
            _jobScheduler = new PrioritizedJobScheduler<long>(
                new CycledSchedulingContextFactory(),
                new TimeAndResolutionPriorityOptionsQuery<long>(
                    new PolicyScheduleOptionsQuery<long>(
                        new CycledTimeSlotCalculator(), 
                        new DeadlineTimePolicy(new NotFitDimensionsPolicy<long>())),
                    new LinearTimePriorityCalculator(),
                    new LinearResolutionPriorityCalculator(new ResolutionPriorityCalculatorOptions(resolutionThreshold))),
                new BestPriorityOptionChooser<long>());

            return this;
        }

        public CycledPrintingSystemBuilder WithSlackBasedScheduler(double resolutionThreshold = 0.2)
        {
            _jobScheduler = new FullReschedulingJobScheduler<long>(
                new CycledSchedulingContextFactory(),
                new ExecutionStatesJobsExtractor<long>(),
                new PrioritizedScheduleProfilesQuery<long>(
                    new CycledProfileTreeFactory(),
                    new ProfileScheduleOptionsQuery<long>(
                        new PolicyScheduleOptionsQuery<long>(
                            new CycledTimeSlotCalculator(),
                            new DeadlineTimePolicy()),
                        new NotFitDimensionsPolicy<long>(),
                        new LinearResolutionPriorityCalculator(
                            new ResolutionPriorityCalculatorOptions(resolutionThreshold)),
                        new LinearTimePriorityCalculator())),
                new PrioritizedProfilesChooser<long>());

            return this;
        }

        public CycledPrintingSystem Build()
        {
            if (_jobsQueue == null)
            {
                throw new InvalidOperationException($"{nameof(_jobsQueue)} is not initialized.");
            }

            if (_jobScheduler == null)
            {
                throw new InvalidOperationException($"{nameof(_jobScheduler)} is not initialized.");
            }

            if (_printers == null || !_printers.Any())
            {
                throw new InvalidOperationException($"{nameof(_printers)} is not initialized.");
            }

            return new CycledPrintingSystem(
                _jobsQueue,
                _jobScheduler,
                new CycledSystemWorkloadContext(_printers));
        }
    }
}