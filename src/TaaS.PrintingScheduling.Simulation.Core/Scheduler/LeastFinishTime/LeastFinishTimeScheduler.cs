using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.Schedules;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingPolicies;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingProfile;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.LeastFinishTime
{
    public class LeastFinishTimeScheduler<TTime> : IJobsScheduler<TTime>
        where TTime : struct
    {
        private readonly IJobTimeSlotCalculator<TTime> _timeSlotCalculator;
        private readonly ISchedulingPolicy<TTime> _schedulingPolicy;
        
        
        private readonly IPrinterSchedulingContextFactory<TTime> _contextFactory;

        public LeastFinishTimeScheduler(IJobTimeSlotCalculator<TTime> timeSlotCalculator, IPrinterSchedulingContextFactory<TTime> contextFactory)
        {
            _timeSlotCalculator = timeSlotCalculator;
            _contextFactory = contextFactory;
            _schedulingPolicy = new NotFitDimensionsPolicy<TTime>(new WorseResolutionPolicy<TTime>());
        }
        
        public SchedulingResult<TTime> Schedule(
            IEnumerable<JobSpecification<TTime>> incomingJobs, 
            IEnumerable<IPrinterSchedulingState<TTime>> states)
        {
            var printersContext = _contextFactory.CreateFilledContexts(states);
            foreach (var incomingJob in incomingJobs)
            {
                var options = GetScheduleOptions(printersContext, incomingJob);
                var option = ChoseBestOption(options);
                
                if (option is null)
                {
                    throw new InvalidOperationException(
                        $"No available option to schedule incoming job with id: '{incomingJob.Id}'.");
                }
                else
                {
                    var printerContext = printersContext.First(context => context.Printer.Id == option.Printer.Id);
                    printerContext.ApplySchedule(option);
                }
            }

            return new SchedulingResult<TTime>(
                printersContext.ToDictionary(
                    context => context.Printer.Id,
                    context => context.Schedules));
        }
        
        private IReadOnlyCollection<JobSchedule<TTime>> GetScheduleOptions(
            IEnumerable<IPrinterSchedulingContext<TTime>> contexts,
            JobSpecification<TTime> job)
        {
            return contexts
                .Where(state => _schedulingPolicy.IsAllowed(state.Printer, job))
                .Select(state => GetScheduleOption(state, job))
                .ToArray();
        }

        private JobSchedule<TTime> GetScheduleOption(IPrinterSchedulingContext<TTime> context, JobSpecification<TTime> job)
        {
            var timeSlot = _timeSlotCalculator.Calculate(context.Printer, job, context.NextAvailableTime);

            return new JobSchedule<TTime>(context.Printer, job, timeSlot);
        }

        private static JobSchedule<TTime> ChoseBestOption(IReadOnlyCollection<JobSchedule<TTime>> options)
        {
            return options.OrderBy(option => option.TimeSlot.Finish).FirstOrDefault();
        }
    }
}