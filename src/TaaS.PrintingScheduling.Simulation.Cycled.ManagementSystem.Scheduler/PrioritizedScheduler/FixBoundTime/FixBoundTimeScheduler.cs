using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Core.SchedulingPolicy;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Time;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Schedules;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingProfile;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.FixBoundTime
{
    public class FixBoundTimeScheduler<TTime> : IJobsScheduler<TTime> where TTime : struct
    {
        
        private readonly ITimePriorityCalculator<TTime> _timePriorityCalculator;
        private readonly IResolutionPriorityCalculator _resolutionPriorityCalculator;

        private readonly ISchedulingPolicy<TTime> _schedulingPolicy;
        
        private readonly IJobTimeSlotCalculator<TTime> _timeSlotCalculator;
        private readonly IPrinterSchedulingContextFactory<TTime> _contextFactory;
        
        public FixBoundTimeScheduler(
            IJobTimeSlotCalculator<TTime> timeSlotCalculator, 
            ITimePriorityCalculator<TTime> timePriorityCalculator,
            IResolutionPriorityCalculator resolutionPriorityCalculator, 
            IPrinterSchedulingContextFactory<TTime> contextFactory)
        {
            _timeSlotCalculator = timeSlotCalculator;
            _timePriorityCalculator = timePriorityCalculator;
            _resolutionPriorityCalculator = resolutionPriorityCalculator;
            _contextFactory = contextFactory;

            _schedulingPolicy = new NotFitDimensionsPolicy<TTime>();
        }
        
        public SchedulingResult<TTime> Schedule(
            IEnumerable<JobSpecification<TTime>> incomingJobs, 
            IEnumerable<IPrinterSchedulingState<TTime>> states)
        {
            
            /*
             * 1) Compose Scheduling profile based on current printer state
             * 2) [Iterative] Try to schedule jobs[i] to profile and track all new jobs
             */
            var printersContext = _contextFactory.CreateFilledContexts(states);
            foreach (var incomingJob in incomingJobs)
            {
                var options = GetPrioritizedScheduleOptions(printersContext, incomingJob);
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

        private IReadOnlyCollection<PrioritizedJobSchedule<TTime>> GetPrioritizedScheduleOptions(
            IEnumerable<IPrinterSchedulingContext<TTime>> contexts,
            JobSpecification<TTime> job)
        {
            var schedules = GetScheduleOptions(contexts, job);

            var minResolution = schedules.Min(printer => printer.Printer.Resolution);
            var maxResolution = schedules.Max(printer => printer.Printer.Resolution);
            
            var minFinishTime = schedules.Min(schedule => schedule.TimeSlot.Finish);
            var maxFinishTime = schedules.Max(schedule => schedule.TimeSlot.Finish);

            return schedules
                .Select(schedule => GetPrioritizedScheduleOption(
                    schedule, job, minFinishTime, maxFinishTime, minResolution, maxResolution))
                .ToArray();
        }
        
        private IReadOnlyCollection<JobSchedule<TTime>> GetScheduleOptions(
            IEnumerable<IPrinterSchedulingContext<TTime>> contexts,
            JobSpecification<TTime> job)
        {
            return contexts
                .Where(context => _schedulingPolicy.IsAllowed(context.Printer, job))
                .Select(context => GetScheduleOption(context, job))
                .ToArray();
        }

        private JobSchedule<TTime> GetScheduleOption(IPrinterSchedulingContext<TTime> context, JobSpecification<TTime> job)
        {
            var timeSlot = _timeSlotCalculator.Calculate(context.Printer, job, context.NextAvailableTime);

            return new JobSchedule<TTime>(context.Printer, job, timeSlot);
        }
        
        private PrioritizedJobSchedule<TTime> GetPrioritizedScheduleOption(
            JobSchedule<TTime> schedule, JobSpecification<TTime> job, TTime minFinishTime, TTime maxFinishTime, double minResolution, double maxResolution)
        {
            var timePriority = _timePriorityCalculator
                .Calculate(schedule.TimeSlot.Finish, minFinishTime, maxFinishTime);

            var calculatorParameters = new ResolutionPriorityParameters(
                minResolution,
                maxResolution,
                job.Resolution,
                schedule.Printer.Resolution);
            
            var resolutionPriority = _resolutionPriorityCalculator.Calculate(calculatorParameters);

            return new PrioritizedJobSchedule<TTime>(schedule, timePriority, resolutionPriority);
        }
        
        private static PrioritizedJobSchedule<TTime> ChoseBestOption(IReadOnlyCollection<PrioritizedJobSchedule<TTime>> options)
        {
            return options
                .Select(option => (Option: option, Priority: CalculateOptionPriority(option)))
                .OrderByDescending(option => option.Priority)
                .FirstOrDefault()
                .Option;
        }

        private static double CalculateOptionPriority(PrioritizedJobSchedule<TTime> option)
        {
            return (option.Job.PriorityCoefficient * option.ResolutionPriority) + ((1 - option.Job.PriorityCoefficient) * option.TimePriority);
        }
    }
}