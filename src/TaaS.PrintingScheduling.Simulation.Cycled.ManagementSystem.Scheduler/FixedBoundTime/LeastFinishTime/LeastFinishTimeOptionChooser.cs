using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.LeastFinishTime
{
    public class LeastFinishTimeOptionChooser<TTime> : IScheduleOptionChooser<TTime> 
        where TTime : struct
    {
        public ScheduleOption<TTime>? ChoseBestOption(
            IReadOnlyCollection<ScheduleOption<TTime>> options)
        {
            return options.OrderBy(option => option.TimeSlot.Finish).FirstOrDefault();
        }
    }
}