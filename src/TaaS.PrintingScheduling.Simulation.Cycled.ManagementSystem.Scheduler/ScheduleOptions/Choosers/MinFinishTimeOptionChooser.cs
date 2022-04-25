namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Choosers
{
    public class MinFinishTimeOptionChooser<TTime> : IOptionChooser<TTime> 
        where TTime : struct
    {
        public ScheduleOption<TTime>? ChooseOption(IEnumerable<ScheduleOption<TTime>> options)
        {
            return options.MinBy(option => option.TimeSlot.Finish);
        }
    }
}