namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.PriorityCalculation.ResolutionPriorityCalculator
{
    public interface IResolutionPriorityCalculator
    {
        public double Calculate(double printerResolution);
    }
}