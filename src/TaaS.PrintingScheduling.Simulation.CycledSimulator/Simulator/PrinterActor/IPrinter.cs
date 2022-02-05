namespace TaaS.PrintingScheduling.Simulation.ConsoleTool.Simulator.PrintingSystem.Printer
{
    public interface IPrinter
    {
        public int Id { get; }
        
        public bool IsFree { get; }
    }
}