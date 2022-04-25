namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Choosers
{
    public interface IPrioritizedOptionChooser<TTime> where TTime : struct
    {
        public PrioritizedScheduleOption<TTime>? ChooseOption(IEnumerable<PrioritizedScheduleOption<TTime>> options);
    }
}