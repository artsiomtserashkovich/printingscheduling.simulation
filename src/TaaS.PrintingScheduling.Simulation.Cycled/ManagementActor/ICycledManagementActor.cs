using TaaS.PrintingScheduling.Simulation.Cycled.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementActor
{
    public interface ICycledManagementActor
    {
        public bool IsComplete { get; }
        
        public void ExecuteCycle(ICycledSimulationContext cycledContext);
    }
}