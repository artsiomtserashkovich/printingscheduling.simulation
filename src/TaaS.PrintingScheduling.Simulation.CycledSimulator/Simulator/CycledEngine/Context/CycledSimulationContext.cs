namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.CycledEngine.Context
{
    public class CycledSimulationContext : ICycledSimulationContext
    {
        public long CurrentCycle { get; private set; }

        public CycledSimulationContext()
        {
            CurrentCycle = 0;
        }

        public void NextCycle()
        {
            CurrentCycle += 1;
        }
    }
}