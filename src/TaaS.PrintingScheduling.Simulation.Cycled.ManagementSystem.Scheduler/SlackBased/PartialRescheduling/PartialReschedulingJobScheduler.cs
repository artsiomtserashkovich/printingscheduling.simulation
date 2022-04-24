using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.PartialRescheduling
{
    public class PartialReschedulingJobScheduler<TTime> : IJobScheduler<TTime> 
        where TTime : struct
    {
        private readonly PartialReschedulingParameters _parameters;

        private readonly IScheduledJobsQuery<TTime> _scheduledJobsQuery;
        private readonly IJobSequencesGenerator<TTime> _sequencesGenerator;

        private readonly ISchedulingContextFactory<TTime> _contextFactory;


        public PartialReschedulingJobScheduler(PartialReschedulingParameters parameters, IScheduledJobsQuery<TTime> scheduledJobsQuery, ISchedulingContextFactory<TTime> contextFactory, IJobSequencesGenerator<TTime> sequencesGenerator)
        {
            _parameters = parameters;
            _scheduledJobsQuery = scheduledJobsQuery;
            _contextFactory = contextFactory;
            _sequencesGenerator = sequencesGenerator;
        }

        public SchedulingResult<TTime> Schedule(PrintingJob<TTime> job, IEnumerable<PrinterExecutionState<TTime>> states, TTime schedulingTime)
        {
            var schedulingJobs = _scheduledJobsQuery
                .Extract(states, _parameters.LastJobsToRescheduleCount)
                .Concat(new []{ job })
                .ToArray();

            var schedulingSequences = _sequencesGenerator.Generate(schedulingJobs);
            
            var printerContexts = states
                .Select(state => _contextFactory.CreatePartiallyFilledContext(state, schedulingTime, schedulingJobs.Select(j => j.Specification.Id)))
                .ToArray();

            foreach (var schedulingSequence in schedulingSequences)
            {
                
            }
            
        }
    }

    public interface IScheduledJobsQuery<TTime> where TTime : struct
    {
        public IReadOnlyCollection<PrintingJob<TTime>> Extract(
            IEnumerable<PrinterExecutionState<TTime>> states,
            int count);
    }

    public interface IJobSequencesGenerator<TTime> where TTime : struct
    {
        public IReadOnlyCollection<IReadOnlyCollection<PrintingJob<TTime>>> Generate(IEnumerable<PrintingJob<TTime>> jobs);
    }
    
    public class PartialReschedulingParameters
    {
        public int LastJobsToRescheduleCount { get; }
        
        public PartialReschedulingParameters(int lastJobsToRescheduleCount)
        {
            LastJobsToRescheduleCount = lastJobsToRescheduleCount;
        }
    }
}
