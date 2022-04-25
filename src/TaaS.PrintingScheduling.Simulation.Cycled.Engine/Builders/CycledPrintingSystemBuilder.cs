using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.IncomingJobsQueue;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Choosers;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.Backfilling;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.FullRescheduling;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.FullRescheduling.Factory;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Choosers;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Prioritized.PriorityCalculation.Time;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized.SchedulingPolicy;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Queries.Unprioritized.TimeSlotCalculator;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

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

        public CycledPrintingSystemBuilder WithUnprioritizedScheduler()
        {
            _jobScheduler = new UnprioritizedAppendJobScheduler<long>(
                new PolicyBasedOptionsQuery<long>(
                    new CycledTimeSlotCalculator(), 
                    new DeadlineTimePolicy(new NotFitDimensionsPolicy<long>(new WorseResolutionPolicy<long>()))),
                new MinFinishTimeOptionChooser<long>(),
                new CycledSchedulingContextFactory());

            return this;
        }

        public CycledPrintingSystemBuilder WithPrioritizedScheduler(double resolutionThreshold = 0.2)
        {
            _jobScheduler = new PrioritizedAppendJobScheduler<long>(
                new CycledSchedulingContextFactory(),
                new TimeScopedPrioritizedOptionsQuery<long>(
                    new PolicyBasedOptionsQuery<long>(
                        new CycledTimeSlotCalculator(), 
                        new DeadlineTimePolicy(new NotFitDimensionsPolicy<long>())),
                    new LinearTimePriorityCalculator(),
                    new LinearResolutionPriorityCalculator(new ResolutionPriorityCalculatorOptions(resolutionThreshold))),
                new MaxTotalPriorityOptionChooser<long>());

            return this;
        }

        public CycledPrintingSystemBuilder WithFullReschedulingScheduler(double resolutionThreshold = 0.2)
        {
            _jobScheduler = new ProfilesBasedJobScheduler<long>(
                new CycledSchedulingContextFactory(),
                new TreeFullReschedulingProfilesQuery<long>(
                    new CycledPrioritizedProfileNodeFactory(),
                    new TimeUnscopedPrioritizedOptionsQuery<long>(
                        new PolicyBasedOptionsQuery<long>(
                            new CycledTimeSlotCalculator(),
                            new DeadlineTimePolicy()),
                        new NotFitDimensionsPolicy<long>(),
                        new LinearResolutionPriorityCalculator(
                            new ResolutionPriorityCalculatorOptions(resolutionThreshold)),
                        new LinearTimePriorityCalculator())),
                new MaxTotalPriorityProfilesChooser<long>());

            return this;
        }

        public CycledPrintingSystemBuilder WithBackfillingScheduler(double resolutionThreshold = 0.2)
        {
            _jobScheduler = new ProfilesBasedJobScheduler<long>(
                new CycledSchedulingContextFactory(),
                new BackfillingProfilesQuery<long>(
                    new GenerateSequencesCommand<long>(),
                    new CycledBackfillingProfileFactory(),
                    new TimeUnscopedPrioritizedOptionsQuery<long>(
                        new PolicyBasedOptionsQuery<long>(
                            new CycledTimeSlotCalculator(),
                            new DeadlineTimePolicy()),
                        new NotFitDimensionsPolicy<long>(),
                        new LinearResolutionPriorityCalculator(
                            new ResolutionPriorityCalculatorOptions(resolutionThreshold)),
                        new LinearTimePriorityCalculator()),
                    new MaxTotalPriorityOptionChooser<long>()),
                new MaxTotalPriorityProfilesChooser<long>());

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