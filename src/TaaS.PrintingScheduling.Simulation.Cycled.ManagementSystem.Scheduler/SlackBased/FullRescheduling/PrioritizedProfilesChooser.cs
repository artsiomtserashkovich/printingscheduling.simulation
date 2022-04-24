using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased;

public class PrioritizedProfilesChooser<TTime> : IPrioritizedProfilesChooser<TTime> where TTime : struct
{
    public PrioritizedSchedulesProfile<TTime>? Chose(IEnumerable<PrioritizedSchedulesProfile<TTime>> profiles)
    {
        return profiles.MaxBy(profile => profile.TotalPriority);
    }
}