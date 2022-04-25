namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Choosers;

public class MaxTotalPriorityProfilesChooser<TTime> : IPrioritizedProfilesChooser<TTime> where TTime : struct
{
    public IPrioritizedOptionsProfile<TTime>? ChooseProfile(IEnumerable<IPrioritizedOptionsProfile<TTime>> profiles)
    {
        return profiles.MaxBy(profile => profile.TotalPriority);
    }
}