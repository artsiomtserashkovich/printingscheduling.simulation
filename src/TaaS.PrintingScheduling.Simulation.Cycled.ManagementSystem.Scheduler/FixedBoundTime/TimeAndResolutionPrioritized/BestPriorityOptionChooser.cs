namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.FixedBoundTime.TimeAndResolutionPrioritized
{
    public class BestPriorityOptionChooser<TTime> : IPrioritizedScheduleOptionChooser<TTime> where TTime : struct
    {
        public PrioritizedScheduleOption<TTime>? ChoseBestOption(IReadOnlyCollection<PrioritizedScheduleOption<TTime>> options)
        {
            return options
                .Select(option => (Option: option, Priority: CalculateOptionPriority(option)))
                .OrderByDescending(option => option.Priority)
                .FirstOrDefault()
                .Option;
        }

        private static double CalculateOptionPriority(PrioritizedScheduleOption<TTime> option)
        {
            return (option.Job.PriorityCoefficient * option.ResolutionPriority) + ((1 - option.Job.PriorityCoefficient) * option.TimePriority);
        }
    } 
}

