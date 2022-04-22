using TaaS.PrintingScheduling.Simulation.Core.Specifications;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Context;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingProfile.Queue;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.SchedulingContext
{
    public abstract class PrinterSchedulingContext<TTime> : IPrinterSchedulingState<TTime> 
        where TTime : struct
    {
        protected readonly ISchedulesQueue<TTime> SchedulesQueue;

        protected PrinterSchedulingContext(
            PrinterSpecification printer,
            ISchedulesQueue<TTime> schedulesQueue)
        {
            Printer = printer;
            SchedulesQueue = schedulesQueue;
        }

        public TTime NextAvailableTime { get; protected set; }

        public PrinterSpecification Printer { get; }

        public IReadOnlySchedulesQueue<TTime> Schedules => SchedulesQueue;

        public abstract void ApplySchedule(JobSchedule<TTime> schedule);
    }

    public class CycledPrinterSchedulingContext : PrinterSchedulingContext<long>
    {
        public CycledPrinterSchedulingContext(
            PrinterSpecification printer, 
            long nextAvailableTime, 
            ISchedulesQueue<long> schedulesQueue) 
            : base(printer, schedulesQueue)
        {
            NextAvailableTime = nextAvailableTime;
        }
        
        public override void ApplySchedule(JobSchedule<long> schedule)
        {
            SchedulesQueue.Enqueue(schedule);

            var jonDuration = schedule.ScheduleTimeSlot.Finish - schedule.ScheduleTimeSlot.Start;
            NextAvailableTime += jonDuration;
        }
    }
}