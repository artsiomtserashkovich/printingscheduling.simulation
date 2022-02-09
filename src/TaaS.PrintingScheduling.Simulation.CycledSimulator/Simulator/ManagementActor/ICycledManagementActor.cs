using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.ManagementActor
{
    public interface ICycledManagementActor
    {
        public bool IsComplete { get; }
        
        public void ExecuteManagingCycle(ICycledSimulationContext cycledContext);
    }
}