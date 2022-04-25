namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions.Choosers;

public interface IOptionChooser<TTime> where TTime : struct
{
    public ScheduleOption<TTime>? ChooseOption(IEnumerable<ScheduleOption<TTime>> options);
}