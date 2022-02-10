using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.Result;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingPolicies;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.LeastFinishTime
{
    public class LeastFinishTimeScheduler<TTime> : IJobsScheduler<TTime>
        where TTime : struct
    {
        private readonly IJobTimeSlotCalculator<TTime> _timeSlotCalculator;
        private readonly ISchedulingPolicy<TTime> _schedulingPolicy;

        public LeastFinishTimeScheduler(IJobTimeSlotCalculator<TTime> timeSlotCalculator)
        {
            _timeSlotCalculator = timeSlotCalculator;
            _schedulingPolicy = new NotFitDimensionsPolicy<TTime>(new WorseResolutionPolicy<TTime>());
        }
        
        public SchedulingResult<TTime> Schedule(
            IEnumerable<JobSpecification<TTime>> incomingJobs, 
            IEnumerable<IPrinterSchedulingState<TTime>> currentState)
        {
            
            /*
             * 1) Compose Scheduling profile based on current printer state
             * 2) [Iterative] Try to schedule jobs[i] to profile and track all new jobs
             */
            
            foreach (var incomingJob in incomingJobs)
            {
                var options = GetScheduleOptions(currentState, incomingJob);
                var option = ChoseBestOption(options);
                if (option is null)
                {
                    throw new InvalidOperationException(
                        $"No available option to schedule incoming job with id: '{incomingJob.Id}'.");
                }
                
                option.State.Schedules.Enqueue(new JobSchedule<TTime>(incomingJob, option.ScheduledTimeSlot));
            }
        }
        
        private IReadOnlyCollection<ScheduleOption<TTime>> GetScheduleOptions(
            IEnumerable<IPrinterSchedulingState<TTime>> states,
            JobSpecification<TTime> job)
        {
            return states
                .Where(state => _schedulingPolicy.IsAllowed(state.Printer, job))
                .Select(state => GetScheduleOption(state, job))
                .ToArray();
        }

        private ScheduleOption<TTime> GetScheduleOption(IPrinterSchedulingState<TTime> state, JobSpecification<TTime> job)
        {
            var lastJobFinishTime = state.Schedules.Any() 
                ? state.Schedules.Last().TimeSlot.Finish 
                : state.NextSlotStartTime;
            
            var timeSlot = _timeSlotCalculator.Calculate(state.Printer, job, lastJobFinishTime);

            return new ScheduleOption<TTime>(state, timeSlot);
        }

        private static ScheduleOption<TTime> ChoseBestOption(IReadOnlyCollection<ScheduleOption<TTime>> options)
        {
            return options.OrderBy(option => option.ScheduledTimeSlot.Start).FirstOrDefault();
        }
    }
}