using TaaS.PrintingScheduling.Simulation.Core.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution;

namespace TaaS.PrintingScheduling.Simulation.Cycled.ManagementSystem.Scheduler.PrioritizedScheduler.PriorityCalculation.Resolution
{
    public interface IResolutionPriorityCalculator
    {
        public double Calculate(ResolutionPriorityParameters parameters);
    }
}