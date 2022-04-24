using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SlackBased.ProfilesGeneration.ProfilesTree
{
    public interface IProfilesTreeFactory<TTime> where TTime : struct
    {
        public IProfileNode<TTime> CreateTree(IEnumerable<IPrinterSchedulingState<TTime>> states);
    }

    public class CycledProfileTreeFactory : IProfilesTreeFactory<long>
    {
        public IProfileNode<long> CreateTree(IEnumerable<IPrinterSchedulingState<long>> states)
        {
            var nodeStates = states
                .Select(state => new CycledNodeSchedulingState(state.Printer, state.NextAvailableTime))
                .ToArray();
            
            return new CycledProfileNode(nodeStates);
        }
    }
}
