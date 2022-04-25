namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Choosers
{
    public interface IPrioritizedProfilesChooser<TTime> where TTime : struct
    {
        public IPrioritizedOptionsProfile<TTime>? ChooseProfile(IEnumerable<IPrioritizedOptionsProfile<TTime>> profiles);
    }
}

