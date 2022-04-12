using System;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Schedules
{
    public class PrioritizedJobSchedule<TTime> : JobSchedule<TTime> where TTime : struct 
    {
        public double TimePriority { get; }
        
        public double ResolutionPriority { get; }

        public PrioritizedJobSchedule(
            JobSchedule<TTime> schedule,  
            double timePriority,
            double resolutionPriority)
            : this(schedule.Printer, schedule.Job, schedule.TimeSlot, timePriority, resolutionPriority)
        { }

        public PrioritizedJobSchedule(
            PrinterSpecification printer, 
            JobSpecification<TTime> job, 
            TimeSlot<TTime> timeSlot,
            double timePriority,
            double resolutionPriority) 
            : base(printer, job, timeSlot)
        {
            if (timePriority < 0 || timePriority > 1)
            {
                throw new ArgumentException(
                    $"Priority can't be less then 0 or more then 1. Current value:'{timePriority}'.",
                    nameof(timePriority));
            }

            if (resolutionPriority < 0 || resolutionPriority > 1)
            {
                throw new ArgumentException(
                    $"Priority can't be less then 0 or more then 1. Current value:'{resolutionPriority}'.",
                    nameof(resolutionPriority));
            }
            
            TimePriority = timePriority;
            ResolutionPriority = resolutionPriority;
        }
    }
}