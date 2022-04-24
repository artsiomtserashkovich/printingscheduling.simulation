using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased
{
    public interface IPrioritizedProfilesChooser<TTime> where TTime : struct
    {
        public PrioritizedSchedulesProfile<TTime>? Chose(IEnumerable<PrioritizedSchedulesProfile<TTime>> profiles);
    }
}

