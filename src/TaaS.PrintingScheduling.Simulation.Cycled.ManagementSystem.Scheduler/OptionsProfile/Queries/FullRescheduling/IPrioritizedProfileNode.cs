using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.OptionsProfile;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.ScheduleOptions;
using TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.SchedulingContext;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.Rescheduling.FullRescheduling.ProfilesGeneration.ProfilesTree.Node
{ 
    public interface IPrioritizedProfileNode<TTime> : IPrioritizedOptionsProfile<TTime> where TTime : struct
    {
        public IReadOnlyCollection<IPrioritizedProfileNode<TTime>> Childs { get; }
        
        public IReadOnlyCollection<IPrinterSchedulingState<TTime>> NodeStates { get; }

        public void CreateChild(PrioritizedScheduleOption<TTime> option);
    }   
}