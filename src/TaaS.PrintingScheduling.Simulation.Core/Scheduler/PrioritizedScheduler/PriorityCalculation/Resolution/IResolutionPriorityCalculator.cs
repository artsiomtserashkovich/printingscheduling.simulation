namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution
{
    public interface IResolutionPriorityCalculator
    {
        public double Calculate(
            double minimumPrinterResolution, 
            double maximumPrinterResolution,
            double expectedResolution,
            double printerResolution);
    }
}