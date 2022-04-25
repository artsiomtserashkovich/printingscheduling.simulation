namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Choosers
{
    public class MaxTotalPriorityOptionChooser<TTime> : IPrioritizedOptionChooser<TTime> where TTime : struct
    {
        public PrioritizedScheduleOption<TTime>? ChooseOption(IEnumerable<PrioritizedScheduleOption<TTime>> options)
        {
            return options.MaxBy(option => option.TotalPriority);
        }
    } 
}

