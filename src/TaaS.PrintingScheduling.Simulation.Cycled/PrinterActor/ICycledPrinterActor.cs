using TaaS.PrintingScheduling.Simulation.Cycled.Context;

namespace TaaS.PrintingScheduling.Simulation.Cycled.PrinterActor
{
    public interface ICycledPrinterActor
    {
        void ExecuteCycle(ICycledSimulationContext simulationContext);
    }
}