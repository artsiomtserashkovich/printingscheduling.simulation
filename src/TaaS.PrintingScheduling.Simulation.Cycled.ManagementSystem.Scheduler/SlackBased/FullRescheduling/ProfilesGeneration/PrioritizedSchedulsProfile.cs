namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration
{
    public class PrioritizedSchedulesProfile<TTime> where TTime : struct
    {
        public IReadOnlyCollection<IPrinterProfileSchedulingState<TTime>> States { get; }
        
        public double TotalPriority { get; }

        public PrioritizedSchedulesProfile(
            IReadOnlyCollection<IPrinterProfileSchedulingState<TTime>> states, 
            double totalPriority)
        {
            States = states;
            TotalPriority = totalPriority;
        }
            
    }
}