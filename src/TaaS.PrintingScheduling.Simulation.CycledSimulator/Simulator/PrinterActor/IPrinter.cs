namespace TaaS.PrintingScheduling.Simulation.CycledSimulator.Simulator.PrinterActor
{
    public interface IPrinter
    {
        public int Id { get; }

        public bool IsFree { get; }
    }
}