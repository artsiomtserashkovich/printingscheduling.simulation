using TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context;

namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor
{
    public interface ICycledPrinterActor
    {
        void ExecuteCycle(ICycledSimulationContext cycledSimulationContext);
    }
}