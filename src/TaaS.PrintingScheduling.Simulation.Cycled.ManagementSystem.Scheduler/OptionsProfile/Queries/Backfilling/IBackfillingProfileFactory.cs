using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.Backfilling
{
    public interface IBackfillingProfileFactory<TTime> where TTime : struct
    {
        public BackfillingPrioritizedOptionsProfile<TTime> 
            CreateFromStates(IEnumerable<IPrinterSchedulingState<TTime>> states);
    }   
    
    public class CycledBackfillingProfileFactory : IBackfillingProfileFactory<long>
    {
        public BackfillingPrioritizedOptionsProfile<long> CreateFromStates(IEnumerable<IPrinterSchedulingState<long>> states)
        {
            var nodeStates = states
                .Select(state => new CycledBackfillingOptionsProfile.CycledSchedulingState(state.Printer, state.NextAvailableTime))
                .ToArray();

            return new CycledBackfillingOptionsProfile(nodeStates);
        }
    }
}