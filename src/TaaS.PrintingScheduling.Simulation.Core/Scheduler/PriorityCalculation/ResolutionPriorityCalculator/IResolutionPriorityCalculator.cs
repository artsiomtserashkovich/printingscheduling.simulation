namespace TaaS.PrintingScheduling.Simulation.Core.Scheduler.PriorityCalculation.ResolutionPriorityCalculator
{
    internal interface IResolutionPriorityCalculator
    {
        public double Calculate(double printerResolution);
    }
}