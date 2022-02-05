namespace TaaS.PrintingScheduling.Simulation.Core.PriorityCalculation.ResolutionPriorityCalculator
{
    internal interface IResolutionPriorityCalculator
    {
        public double Calculate(double printerResolution);
    }
}