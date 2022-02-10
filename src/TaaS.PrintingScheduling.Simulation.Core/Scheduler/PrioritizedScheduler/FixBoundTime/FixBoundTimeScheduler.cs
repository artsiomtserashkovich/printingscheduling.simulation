using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.PriorityCalculation.Time;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.Result;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingPolicies;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.FixBoundTime
{
    public class FixBoundTimeScheduler<TTime> : IJobsScheduler<TTime> where TTime : struct
    {
        private readonly IJobTimeSlotCalculator<TTime> _timeSlotCalculator;
        private readonly ITimePriorityCalculator<TTime> _timePriorityCalculator;
        private readonly IResolutionPriorityCalculator _resolutionPriorityCalculator;

        private readonly ISchedulingPolicy<TTime> _schedulingPolicy;
        
        public FixBoundTimeScheduler(
            IJobTimeSlotCalculator<TTime> timeSlotCalculator, 
            ITimePriorityCalculator<TTime> timePriorityCalculator,
            IResolutionPriorityCalculator resolutionPriorityCalculator)
        {
            _timeSlotCalculator = timeSlotCalculator;
            _timePriorityCalculator = timePriorityCalculator;
            _resolutionPriorityCalculator = resolutionPriorityCalculator;

            _schedulingPolicy = new NotFitDimensionsPolicy<TTime>();
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
                var options = GetPrioritizedScheduleOptions(currentState, incomingJob);
                var option = ChoseBestOption(options);
                if (option is null)
                {
                    throw new InvalidOperationException(
                        $"No available option to schedule incoming job with id: '{incomingJob.Id}'.");
                }
                
                option.State.Schedules.Enqueue(new JobSchedule<TTime>(incomingJob, option.ScheduledTimeSlot));
            }
        }

        private IReadOnlyCollection<PrioritizedScheduleOption<TTime>> GetPrioritizedScheduleOptions(
            IEnumerable<IPrinterSchedulingState<TTime>> states,
            JobSpecification<TTime> job)
        {
            var schedules = GetScheduleOptions(states, job);

            var minResolution = schedules.Min(printer => printer.State.Printer.Resolution);
            var maxResolution = schedules.Max(printer => printer.State.Printer.Resolution);
            
            var minFinishTime = schedules.Min(schedule => schedule.ScheduledTimeSlot.Finish);
            var maxFinishTime = schedules.Max(schedule => schedule.ScheduledTimeSlot.Finish);

            return schedules
                .Select(schedule => GetPrioritizedScheduleOption(
                    schedule, job, minFinishTime, maxFinishTime, minResolution, maxResolution))
                .ToArray();
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
        
        private PrioritizedScheduleOption<TTime> GetPrioritizedScheduleOption(
            ScheduleOption<TTime> schedule, JobSpecification<TTime> job, TTime minFinishTime, TTime maxFinishTime, double minResolution, double maxResolution)
        {
            var timePriority = _timePriorityCalculator
                .Calculate(schedule.ScheduledTimeSlot.Finish, minFinishTime, maxFinishTime);

            var resolutionPriority = _resolutionPriorityCalculator
                .Calculate(
                    minResolution,
                    maxResolution,
                    job.Resolution,
                    schedule.State.Printer.Resolution);

            return new PrioritizedScheduleOption<TTime>(schedule, timePriority, resolutionPriority);
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