using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Scheduler.SchedulingPolicies;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.LeastFinishTime
{
    public class LeastFinishTimeScheduler<TTime> : IJobsScheduler<TTime>
        where TTime : struct
    {
        private readonly IJobScheduleFactory<TTime> _scheduleFactory;
        private readonly ISchedulingPolicy<TTime> _schedulingPolicy;

        public LeastFinishTimeScheduler(IJobScheduleFactory<TTime> scheduleFactory)
        {
            _scheduleFactory = scheduleFactory;
            _schedulingPolicy = new NotFitDimensionsPolicy<TTime>(new WorseResolutionPolicy<TTime>());
        }
        
        public IReadOnlyCollection<(PrinterSpecification Printer, IReadOnlyCollection<JobSchedule<TTime>> Schedules)> 
            Schedule(
                IReadOnlyCollection<JobSpecification<TTime>> incomingJobs, 
                IReadOnlyCollection<IPrinterExecutionState<TTime>> printerExecutionStates)
        {
            
            /*
             * 1) Compose Scheduling profile based on current printer state
             * 2) [Iterative] Try to schedule jobs[i] to profile and track all new jobs
             * 3) Calculate diff and return or return whole profile state
             */
            
            var jobScheduleOption = GetScheduleOption(printerExecutionStates, job.Specification);
            if (jobScheduleOption is null)
            {
                throw new InvalidOperationException(
                    $"No available options to schedule incoming job with id: '{job.Specification.Id}'.");
            }

            var printersSchedules = new List<(PrinterSpecification Printer, IReadOnlyCollection<JobSchedule<TTime>>)>();
            foreach (var printerState in printerExecutionStates)
            {
                var previousPrinterSchedules = printerState.Schedules;
                if (printerState.Printer.Id == jobScheduleOption.Printer.Id)
                {
                    var jobSchedule = _scheduleFactory.Schedule(jobScheduleOption.Printer, job, jobScheduleOption.StartSlotTime);
                    
                    previousPrinterSchedules = previousPrinterSchedules.Append(jobSchedule).ToArray();
                }
                
                printersSchedules.Add((printerState.Printer, previousPrinterSchedules));
            }
            
            return printersSchedules;
        }

        private ScheduleOption GetScheduleOption(
            IReadOnlyCollection<IPrinterExecutionState<TTime>> printersStates, JobSpecification<TTime> job)
        {
            var allowedPrinters = printersStates
                .Where(state => _schedulingPolicy.IsAllowed(state.Printer, job));

            return allowedPrinters
                .Select(state => new ScheduleOption(state.Printer, GetStartSlotTime(state.Schedules)))
                .OrderBy(option => option.StartSlotTime)
                .FirstOrDefault();
        }

        private TTime GetStartSlotTime(IReadOnlyCollection<JobSchedule<TTime>> schedules)
        {
            return schedules.Select(schedule => schedule.ExpectedFinishTime).Max();
        }

        private class ScheduleOption
        {
            public ScheduleOption(PrinterSpecification printer, TTime startSlotTime)
            {
                Printer = printer;
                StartSlotTime = startSlotTime;
            }
            
            public PrinterSpecification Printer { get; }
            
            public TTime StartSlotTime { get; }
        }
    }

    public interface IJobScheduleFactory<TTime> where TTime : struct
    {
        JobSchedule<TTime> Schedule(PrinterSpecification printer, IPrintingJob<TTime> job, TTime slotStart);
    }
}