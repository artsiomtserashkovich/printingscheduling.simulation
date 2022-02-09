using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PriorityCalculation.ResolutionPriorityCalculator;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PriorityCalculation.TimePriorityCalculator;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingPolicies;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.FixBoundTime
{
    public class FixBoundTimeScheduler<TTime> : IJobsScheduler<TTime> where TTime : struct
    {
        private readonly IJobTimeSlotCalculator<TTime> _timeSlotCalculator;
        private readonly ITimePriorityCalculator<TTime> _timePriorityCalculator;

        private readonly ISchedulingPolicy<TTime> _schedulingPolicy;
        
        public FixBoundTimeScheduler(
            IJobTimeSlotCalculator<TTime> timeSlotCalculator, 
            ITimePriorityCalculator<TTime> timePriorityCalculator)
        {
            _timeSlotCalculator = timeSlotCalculator;
            _timePriorityCalculator = timePriorityCalculator;

            _schedulingPolicy = new NotFitDimensionsPolicy<TTime>();
        }
        
        public void Schedule(
            IEnumerable<JobSpecification<TTime>> incomingJobs, 
            IEnumerable<IPrinterSchedulingState<TTime>> currentState, 
            TTime currentTime)
        {
            
            /*
             * 1) Compose Scheduling profile based on current printer state
             * 2) [Iterative] Try to schedule jobs[i] to profile and track all new jobs
             */
            
            foreach (var incomingJob in incomingJobs)
            {
                var options = GetScheduleOption(currentState, currentTime, incomingJob);
                var option = ChoseBestOption(options);
                if (option is null)
                {
                    throw new InvalidOperationException(
                        $"No available option to schedule incoming job with id: '{incomingJob.Id}'.");
                }
                
                option.State.Schedules.Enqueue(new JobSchedule<TTime>(incomingJob, option.ScheduledTimeSlot));
            }
        }

        private IReadOnlyCollection<PrioritizedScheduleOption<TTime>> GetScheduleOption(
            IEnumerable<IPrinterSchedulingState<TTime>> currentState,
            TTime currentTime,
            JobSpecification<TTime> job)
        {
            var allowedPrintersStates = currentState
                .Where(state => _schedulingPolicy.IsAllowed(state.Printer, job));

            if (!allowedPrintersStates.Any())
            {
                return Array.Empty<PrioritizedScheduleOption<TTime>>();
            }

            var minResolution = allowedPrintersStates.Min(printer => printer.Printer.Resolution);
            var maxResolution = allowedPrintersStates.Max(printer => printer.Printer.Resolution);
            // TODO: move params to method (not init everyTime)
            var resolutionPriorityCalculator = new LinearResolutionPriorityCalculator(
                minResolution, maxResolution, job.Resolution, 0.2);
            
            var schedules =  allowedPrintersStates
                .Select(state => new PrioritizedScheduleOption<TTime>(
                    state, 
                    GetNextTimeSlot(state,currentTime, job), 
                    resolutionPriorityCalculator.Calculate(state.Printer.Resolution)))
                .ToArray();

            var minFinishTime = schedules.Min(schedule => schedule.ScheduledTimeSlot.Finish);
            var maxFinishTime = schedules.Max(schedule => schedule.ScheduledTimeSlot.Finish);
            foreach (var schedule in schedules)
            {
                schedule.TimePriority = _timePriorityCalculator
                    .Calculate(
                        schedule.ScheduledTimeSlot.Finish,
                        minFinishTime,
                        maxFinishTime);
            }

            return schedules;
        }

        private TimeSlot<TTime> GetNextTimeSlot(
            IPrinterSchedulingState<TTime> state,
            TTime currentTime,
            JobSpecification<TTime> job)
        {
            var lastJobFinishTime = state.Schedules.Any() ? state.Schedules.Last().TimeSlot.Finish : currentTime;

            return _timeSlotCalculator.Calculate(state.Printer, job, lastJobFinishTime);
        }
        
        private static PrioritizedScheduleOption<TTime> ChoseBestOption(IReadOnlyCollection<PrioritizedScheduleOption<TTime>> options)
        {
            var coef = 0.5;
            
            return options
                .Select(option => (option, (coef * option.ResolutionPriority) + (1 - coef) * option.TimePriority))
                .OrderByDescending(option => option.Item2)
                .FirstOrDefault()
                .option;
        }
    }
}