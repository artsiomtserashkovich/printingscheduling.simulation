using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile.Queries.FullRescheduling;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Rescheduling.FullRescheduling.ProfilesGeneration.ProfilesTree.Node.Factory
{
    public interface IPrioritizedProfileNodeFactory<TTime> where TTime : struct
    {
        public PrioritizedProfileNode<TTime> CreateRootNode(IEnumerable<IPrinterSchedulingState<TTime>> initialStates);
    }
}

