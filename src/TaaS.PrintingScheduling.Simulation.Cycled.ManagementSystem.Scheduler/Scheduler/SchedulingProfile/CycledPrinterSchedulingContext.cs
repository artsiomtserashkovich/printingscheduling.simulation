using System;
using System.Collections.Generic;
using System.Linq;
using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Schedules;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingProfile
{
    public class CycledPrinterSchedulingContext : IPrinterSchedulingContext<long>
    {
        private readonly Queue<JobSchedule<long>> _schedules;
                
        public CycledPrinterSchedulingContext(PrinterSpecification printer, long nextAvailableTime)
        {
            _schedules = new Queue<JobSchedule<long>>();
            Printer = printer;
            NextAvailableTime = nextAvailableTime;
        }

        private CycledPrinterSchedulingContext(PrinterSpecification printer, long nextAvailableTime, Queue<JobSchedule<long>> currentScheduling)
        {
            _schedules = currentScheduling;
            Printer = printer;
            NextAvailableTime = nextAvailableTime;
        }

        public long NextAvailableTime { get; private set; }

        public PrinterSpecification Printer { get; }

        public IReadOnlyCollection<JobSchedule<long>> Schedules => _schedules.ToArray();

        public void ApplySchedule(JobSchedule<long> schedule)
        {
            if (schedule.Printer.Id != Printer.Id)
            {
                throw new ArgumentException($"Not correct printer id: '{schedule.Printer.Id}', expected: '{Printer.Id}'", nameof(schedule));
            }
            if (_schedules.Any() && _schedules.Last().TimeSlot.Finish >= schedule.TimeSlot.Start)
            {
                throw new ArgumentException(
                    $"Job schedule start time: '{_schedules.Last().TimeSlot.Finish}' overlap with last previous schedule finish time: '{schedule.TimeSlot.Start}'.", 
                    nameof(schedule));
            }
            
            _schedules.Enqueue(schedule);
            NextAvailableTime = schedule.TimeSlot.Finish + 1;
        }

        public static CycledPrinterSchedulingContext FromState(IPrinterSchedulingState<long> state)
        {
            var nextAvailableTime = state.Schedules.Any()
                ? state.Schedules.Last().TimeSlot.Finish + 1
                : state.NextSlotStartTime;
            
            return new CycledPrinterSchedulingContext(state.Printer, nextAvailableTime, new Queue<JobSchedule<long>>(state.Schedules));
        }
    }
}