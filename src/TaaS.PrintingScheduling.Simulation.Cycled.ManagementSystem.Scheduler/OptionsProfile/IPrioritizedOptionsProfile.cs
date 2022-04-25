using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile
{
    public interface IPrioritizedOptionsProfile<TTime> where TTime : struct
    {
        public double TotalPriority { get; }

        public IReadOnlyCollection<PrioritizedScheduleOption<TTime>> Options { get; }
    }
}

