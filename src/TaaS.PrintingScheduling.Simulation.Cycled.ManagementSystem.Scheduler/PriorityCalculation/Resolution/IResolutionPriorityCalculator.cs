namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution
{
    public interface IResolutionPriorityCalculator
    {
        public double Calculate(ResolutionPriorityParameters parameters);
    }
}