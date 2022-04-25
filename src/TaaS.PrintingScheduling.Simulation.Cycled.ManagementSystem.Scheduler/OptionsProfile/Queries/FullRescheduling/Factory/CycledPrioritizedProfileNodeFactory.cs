using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Rescheduling.FullRescheduling.ProfilesGeneration.ProfilesTree.Node.Factory;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.FullRescheduling.Factory
{
    public class CycledPrioritizedProfileNodeFactory : IPrioritizedProfileNodeFactory<long>
    {
        public PrioritizedProfileNode<long> CreateRootNode(IEnumerable<IPrinterSchedulingState<long>> initialStates)
        {
            var nodeStates = initialStates
                .Select(state => new CycledPrioritizedPrioritizedProfileNode.CycledNodeFlatSchedulingState(state.Printer, state.NextAvailableTime))
                .ToArray();
            
            return new CycledPrioritizedPrioritizedProfileNode(nodeStates);
        }
    }
}

